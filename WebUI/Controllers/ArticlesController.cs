using Domain.Entities;
using Domain.Context;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using WebUI.Models;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Xml.Serialization;

namespace WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private int PageSize = 20;
        private EFConstantContext _constantContext;

        public ArticlesController()
        {
        }

        public ArticlesController(EFConstantContext constantContext)
        {
            ConstantContext = constantContext;
        }

        public EFConstantContext ConstantContext
        {
            get
            {
                return _constantContext ?? new EFConstantContext();
            }
            private set
            {
                _constantContext = value;
            }
        }

        [Authorize(Roles = "Администратор, Редактор")]
        public ActionResult Index(int page = 1)
        {
            return View(GetListArticlesModel(page));
        }

        [Authorize(Roles = "Администратор, Редактор")]
        public async Task<ActionResult> EditArticle(Guid Id)
        {
            Article model = null;
            using (EFArticleContext articleContext = new EFArticleContext())
            {
                model = await articleContext.FindByIdAsync(Id);
            }
            if (model != null)
            {
                model.TextArticle = Server.HtmlDecode(model.TextArticle);
                model.TextMain = Server.HtmlDecode(model.TextMain);
            }
            else
            {
                model = new Domain.Entities.Article();
                model.IsVisible = true;
                model.DatePublish = DateTime.Now;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Редактор")]
        public async Task<ActionResult> EditArticle(Article article)
        {
            Guid id = Guid.Empty;
            if (ModelState.IsValid)
            {
                using (EFArticleContext articleContext = new EFArticleContext())
                {
                    article.Link = Regex.Replace(Regex.Replace(Helpers.Texts.Translit(article.Title).ToLower(), @"[^\d\w]", "-").Trim('-'), @"-+", "-");
                    id = await articleContext.SaveArticleAsync(article);

                    //обновляем sitemap
                    await CreateFileSitemapAsync();
                }
                return RedirectToAction("EditArticle/" + id.ToString());
            }
            return RedirectToAction("EditArticle/" + id.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор, Редактор")]
        public async Task<ActionResult> DeleteArticle(Guid ArticleId, int page = 1)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                using (EFArticleContext articleContext = new EFArticleContext())
                {
                    await articleContext.DeleteArticleAsync(ArticleId);

                    //обновляем sitemap
                    await CreateFileSitemapAsync();
                }
            }
            return PartialView("_ListArticle", GetListArticlesModel(page).Articles);
        }

        public async Task CreateFileSitemapAsync()
        {
            TextWriter writer = new StreamWriter(Server.MapPath("~/sitemap.xml"));
            try
            {
                List<url> listUrl = new List<url>();
                //заполняем статическимим ссылками

                listUrl.Add(new url
                {
                    changefreq = "daily",
                    loc = "http://baeroff.com",
                    priority = 1.0
                });

                listUrl.Add(new url
                {
                    changefreq = "monthly",
                    lastmod = Helpers.Services.ConvertDateToW3CTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)),
                    loc = "http://baeroff.com/home/register",
                    priority = 1.0
                });

                //заполняем статьями
                using (EFArticleContext articleContext = new EFArticleContext())
                {
                    List<url> listArticle = await (from article in articleContext.Articles.AsQueryable()
                                                   where article.IsVisible == true
                                                   select new url
                                                   {
                                                       changefreq = "monthly",
                                                       lastmod = article.DatePublish.ToString(),
                                                       loc = "http://baeroff.com/articles/article/" + article.Link,
                                                       priority = 0.8
                                                   }).ToListAsync();
                    foreach (var la in listArticle)
                    {
                        la.lastmod = Helpers.Services.ConvertDateToW3CTime(DateTime.Parse(la.lastmod));
                    }
                    listUrl.AddRange(listArticle);
                }

                urlset data = new urlset();
                data.urls = listUrl;

                XmlSerializer serializer = new XmlSerializer(typeof(urlset));
                serializer.Serialize(writer, data);
            }
            finally
            {
                writer.Close();
            }
        }


        [AllowAnonymous]
        //[OutputCache(Duration = 600, VaryByParam = "none", Location = OutputCacheLocation.Downstream)]
        public async Task<ActionResult> Article(String link)
        {
            Article model = null;
            using (EFArticleContext articleContext = new EFArticleContext())
            {
                model = await articleContext.FindByLinkAsync(link);
            }
            if (model != null)
            {
                model.TextArticle = Server.HtmlDecode(model.TextArticle);
                model.TextMain = Server.HtmlDecode(model.TextMain);

                //добавляем просмотр
                using (EFCountViewContext countViewContext = new EFCountViewContext())
                {
                    if (!User.IsInRole("Администратор") && !User.IsInRole("Редактор"))
                    {
                        await countViewContext.AppendViewAsync(model.Id, Domain.Entities.ViewType.Article);
                    }
                    ViewBag.CountView = await countViewContext.GetCountViewAsync(model.Id, Domain.Entities.ViewType.Article);
                }
                Session["Link"] = "/articles/article/" + model.Link;

                using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
                {
                    Session["Group"] = await menuSetContext.GetGroupByLinkAsync("/articles/article/" + model.Link);
                }
            }
            else
            {
                return HttpNotFound();
            }
            ViewBag.MyUrl = await ConstantContext.GetConstantAsync("Общие: URL сайта");
            ViewBag.MyDomain = await ConstantContext.GetConstantAsync("Общие: имя домена");
            ViewBag.RightPanel = await ConstantContext.GetConstantAsync("Публикации: показывать правую панель");
            return View(model);
        }

        [Authorize(Roles = "Администратор,Менеджер")]
        public PartialViewResult ListArticle(int page = 1)
        {
            return PartialView("_ListArticle", GetListArticlesModel(page).Articles);
        }

        private ArticlesView GetListArticlesModel(int page)
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            {
                int tp = (Int32)Math.Ceiling((decimal)articleContext.Articles.Count() / PageSize);
                page = page < 0 ? 1 : page > tp ? tp : page;

                ArticlesView model = new ArticlesView
                {
                    Articles = articleContext.Articles.Count() == 0 ? articleContext.Articles.ToList() :
                        articleContext.Articles.OrderByDescending(a => a.DatePublish).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemPerPage = PageSize,
                        TotalItems = articleContext.Articles.Count()
                    }
                };
                return model;
            }
        }
    }
}
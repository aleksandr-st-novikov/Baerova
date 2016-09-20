using Domain.Entities;
using Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web.UI;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        public int PageSize = 3;

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
                }
            }
            return PartialView("_ListArticle", GetListArticlesModel(page).Articles);
        }

        [AllowAnonymous]
        [OutputCache(Duration = 600, VaryByParam = "none", Location = OutputCacheLocation.Downstream)]
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
            }
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
                    Articles = articleContext.Articles.OrderByDescending(a => a.DateCreate).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
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
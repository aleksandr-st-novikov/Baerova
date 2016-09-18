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

namespace WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        [Authorize(Roles = "Администратор, Редактор")]
        public ActionResult Index()
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            {
                return View(articleContext.Articles.ToList());
            }
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
    }
}
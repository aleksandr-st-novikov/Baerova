﻿using Domain.Entities;
using Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

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
            }
            return RedirectToAction("EditArticle/" + id.ToString());
        }

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
            }
            return View(model);
        }
    }
}
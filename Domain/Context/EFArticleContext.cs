﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFArticleContext : IDisposable
    {
        private mainDbContext context = new mainDbContext();

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                context.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        public async Task<Article> FindByIdAsync(Guid id)
        {
            return await context.Articles.FindAsync(id);
        }

        public IQueryable<Article> Articles
        {
            get { return context.Articles.AsNoTracking(); }
        }

        public async Task<Guid> SaveArticleAsync(Article article)
        {
            if(article.IsFixed)
            {
                await ClearFixedAsync(article.Category);
            }

            article.DateCreate = DateTime.Now;
            if (article.Id == Guid.Empty)
            {
                article.Id = Guid.NewGuid();
                context.Articles.Add(article);
            }
            else
            {
                Article forChange = await context.Articles.FindAsync(article.Id);
                forChange.DatePublish = article.DatePublish;
                forChange.Descr = article.Descr;
                forChange.IsVisible = article.IsVisible;
                forChange.PictShare = article.PictShare;
                forChange.TextArticle = article.TextArticle;
                forChange.TextMain = article.TextMain;
                forChange.Title = article.Title;
                forChange.Link = article.Link;
                forChange.DateCreate = DateTime.Now;
                forChange.Category = article.Category;
                forChange.IsFixed = article.IsFixed;
            }
            await context.SaveChangesAsync();
            return article.Id;
        }

        public async Task ClearFixedAsync(string category)
        {
            List<Article> forChange = await context.Articles.Where(a => a.IsFixed == true && a.Category == category).ToListAsync();
            if(forChange != null)
            {
                foreach(Article a in forChange)
                {
                    a.IsFixed = false;
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<Article> FindByLinkAsync(string link)
        {
            //return await context.Articles.FirstOrDefaultAsync(a => (a.DatePublish == null || a.DatePublish <= DateTime.Now) && a.Link == link);
            return await context.Articles.FirstOrDefaultAsync(a => a.Link == link);
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            Article forDelele = await context.Articles.FindAsync(id);
            if (forDelele != null)
            {
                context.Articles.Remove(forDelele);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Article>> GetMainArticlesAsync(int pageArticle)
        {
            Constant constant = await context.Constants.Where(c => c.Name == "Главная: количество публикаций").FirstOrDefaultAsync();
            int PageSize = constant == null ? 3 : Int32.Parse(constant.Value.ToString());
            return await context.Articles
                    .Where(a => a.IsVisible == true && !String.IsNullOrEmpty(a.TextMain) && a.DatePublish <= DateTime.Now)
                    .OrderByDescending(a => a.DatePublish)
                    .Skip((pageArticle - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();
        }

        public List<Article> GetArticles(int pageArticle, string category)
        {
            Constant constant = context.Constants.Where(c => c.Name == "Главная: количество публикаций").FirstOrDefault();
            int PageSize = constant == null ? 3 : Int32.Parse(constant.Value.ToString());
            return context.Articles
                    .Where(a => a.IsVisible == true && !String.IsNullOrEmpty(a.TextMain) && a.DatePublish <= DateTime.Now && a.Category == category && a.IsFixed != true)
                    .OrderByDescending(a => a.DatePublish)
                    .Skip((pageArticle - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
        }


        public Article GetFixedArticle(string category)
        {
            if (!String.IsNullOrEmpty(category))
            {
                return context.Articles
                        .Where(a => a.IsVisible == true && !String.IsNullOrEmpty(a.TextMain) && a.DatePublish <= DateTime.Now && a.IsFixed == true && a.Category == category)
                        .FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public int GetArticlesCount(string category)
        {
            return context.Articles.Where(a => a.IsVisible == true && !String.IsNullOrEmpty(a.TextMain) && a.DatePublish <= DateTime.Now && a.IsFixed == false && a.Category == category).Count();
        }

        public List<Article> ArticlesForMailing()
        {
            List<Article> forMailing = (from a in context.Articles
                                        where a.IsVisible == true &&
                                        !String.IsNullOrEmpty(a.TextMain) &&
                                        a.DatePublish <= DateTime.Now &&
                                        !(from ma in context.MailArticles select ma.ArticleId).Contains(a.Id)
                                        orderby a.DatePublish
                                        select a)
                                        .ToList();

            return forMailing;
        }

        public async Task<List<Article>> ArticlesForMailingAsync()
        {
            List<Article> forMailing = await (from a in context.Articles
                                              where a.IsVisible == true &&
                                              !(String.IsNullOrEmpty(a.TextMain)) &&
                                              a.DatePublish <= DateTime.Now &&
                                              !(from ma in context.MailArticles select ma.ArticleId).Contains(a.Id)
                                              orderby a.DatePublish
                                              select a)
                                              .ToListAsync();
            return forMailing;
        }
    }
}

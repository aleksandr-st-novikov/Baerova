using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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


        public IQueryable<Article> Articles
        {
            get { return context.Articles.AsNoTracking(); }
        }

        public async Task<Guid> SaveArticleAsync(Article article)
        {
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
            }
            await context.SaveChangesAsync();
            return article.Id;
        }
    }
}

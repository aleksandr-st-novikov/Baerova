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
            }
            await context.SaveChangesAsync();
            return article.Id;
        }

        public async Task<Article> FindByLinkAsync(string link)
        {
            return await context.Articles.FirstOrDefaultAsync(a => (a.DatePublish == null || a.DatePublish <= DateTime.Now) && a.Link == link);
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

        public List<Article> GetMainArticles(int pageArticle)
        {
            Constant constant = context.Constants.Where(c => c.Name == "Главная: количество публикаций").FirstOrDefault();
            int PageSize = constant == null ? 3 : Int32.Parse(constant.Value.ToString());
            return context.Articles
                    .Where(a => a.IsVisible == true && !String.IsNullOrEmpty(a.TextMain) && a.DatePublish <= DateTime.Now)
                    .OrderByDescending(a => a.DatePublish)
                    .Skip((pageArticle - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
        }
    }
}

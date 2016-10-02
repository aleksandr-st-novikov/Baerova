using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Context
{
    public class EFMailArticleContext : IDisposable
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

        public IQueryable<MailArticle> MailArticles
        {
            get { return context.MailArticles.AsNoTracking(); }
        }

        public async Task SaveMailArticleAsync(MailArticle mailArticle)
        {
            context.MailArticles.Add(mailArticle);
            await context.SaveChangesAsync();
        }
        public void SaveMailArticle(MailArticle mailArticle)
        {
            context.MailArticles.Add(mailArticle);
            context.SaveChanges();
        }

    }
}

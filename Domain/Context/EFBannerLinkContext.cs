using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFBannerLinkContext : IDisposable
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

        public IQueryable<BannerLink> BannerLinks
        {
            get { return context.BannerLinks.AsNoTracking(); }
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="bannerLink"></param>
        /// <returns></returns>
        public async Task SaveBannerLinkAsync(BannerLink bannerLink)
        {
            if (bannerLink.Id == Guid.Empty)
            {
                bannerLink.Id = Guid.NewGuid();
                context.BannerLinks.Add(bannerLink);
            }
            else
            {
                BannerLink forChange = await context.BannerLinks.FindAsync(bannerLink.Id);
                if (forChange != null)
                {
                    forChange.NumSlide = bannerLink.NumSlide;
                    forChange.Link = bannerLink.Link;
                }
            }
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="bannerLinkId"></param>
        /// <returns></returns>
        public async Task DeleteBannerLinkAsync(Guid bannerLinkId)
        {
            BannerLink forDelete = await context.BannerLinks.FindAsync(bannerLinkId);
            if (forDelete != null)
            {
                context.BannerLinks.Remove(forDelete);
            }
            await context.SaveChangesAsync();
        }
    }
}

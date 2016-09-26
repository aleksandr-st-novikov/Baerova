using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFCountViewContext : IDisposable
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
    
        public async Task AppendViewAsync(Guid viewId, ViewType viewType)
        {
            CountView cv = await context.CountViews.FirstOrDefaultAsync(c => c.ViewId == viewId && c.ViewType == viewType);
            if(cv != null)
            {
                cv.ViewCount = cv.ViewCount == null ? 1 : cv.ViewCount + 1;
            }
            else
            {
                cv = new CountView()
                {
                    Id = Guid.NewGuid(),
                    ViewCount = 1,
                    ViewId = viewId,
                    ViewType = viewType
                };
                context.CountViews.Add(cv);
            }
            await context.SaveChangesAsync();
        }

        public async Task<string> GetCountViewAsync(Guid viewId, ViewType viewType)
        {
            var res = await context.CountViews.FirstOrDefaultAsync(c => c.ViewId == viewId && c.ViewType == viewType);
            return res == null ? null : Math.Round((Decimal)res.ViewCount).ToString();
        }

        public string GetCountView(Guid viewId, ViewType viewType)
        {
            var res = context.CountViews.FirstOrDefault(c => c.ViewId == viewId && c.ViewType == viewType);
            return res == null ? null : Math.Round((Decimal)res.ViewCount).ToString();
        }
    }
}

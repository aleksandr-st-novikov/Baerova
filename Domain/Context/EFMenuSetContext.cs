using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFMenuSetContext : IDisposable
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

        public IQueryable<MenuSet> MenuSets
        {
            get
            {
                return context.MenuSets.AsNoTracking();
            }
        }

        /// <summary>
        /// Сохранение настройки меню
        /// </summary>
        /// <param name="menuSet"></param>
        /// <returns></returns>
        public async Task SaveMenuSetAsync(MenuSet menuSet)
        {
            if (menuSet.Id == Guid.Empty)
            {
                menuSet.Id = Guid.NewGuid();
                context.MenuSets.Add(menuSet);
            }
            else
            {
                MenuSet forChange = await context.MenuSets.FindAsync(menuSet.Id);
                if (forChange != null)
                {
                    //forChange.Group = menuSet.Group;
                    forChange.IsActive = menuSet.IsActive;
                    forChange.Link = menuSet.Link;
                    //forChange.Name = menuSet.Name;
                    forChange.Order = menuSet.Order;
                }
            }
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление настройки меню
        /// </summary>
        /// <param name="menuSetId"></param>
        /// <returns></returns>
        public async Task DeleteMenuSetAsync(Guid menuSetId)
        {
            MenuSet forDelete = await context.MenuSets.FindAsync(menuSetId);
            if (forDelete != null)
            {
                context.MenuSets.Remove(forDelete);
            }
            await context.SaveChangesAsync();
        }

        public async Task<string> GetGroupByLinkAsync(string v)
        {
            MenuSet ms = await context.MenuSets.FirstOrDefaultAsync(m => m.Link == v);
            return ms == null ? null : ms.Group.ToString();
        }
    }
}

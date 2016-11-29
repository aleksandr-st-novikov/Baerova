using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFPartnerContext : IDisposable
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

        public IQueryable<Partner> Partners
        {
            get { return context.Partners.AsNoTracking(); }
        }

        public async Task SavePartnerAsync(Partner partner)
        {
            Partner forChange = await context.Partners.FindAsync(partner.Id);
            if (forChange == null)
            {
                partner.Id = Guid.NewGuid();
                partner.DateCreate = DateTime.Now;
                context.Partners.Add(partner);
            }
            else
            {
                forChange.DOB = partner.DOB;
                forChange.EMail = partner.EMail;
                forChange.Name = partner.Name;
                forChange.Patronymic = partner.Patronymic;
                forChange.Phone = partner.Phone;
                forChange.Surname = partner.Surname;
                forChange.IsDiscount = partner.IsDiscount;
                forChange.IsDistributor = partner.IsDistributor;
                forChange.IsShopkeeper = partner.IsShopkeeper;
            }
            await context.SaveChangesAsync();
        }

        public async Task DeletePartnetAsync(Guid id)
        {
            Partner forDelete = await context.Partners.FindAsync(id);
            if(forDelete != null)
            {
                context.Partners.Remove(forDelete);
                await context.SaveChangesAsync();
            }
        }
    }
}

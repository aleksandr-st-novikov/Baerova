using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFSubscriberContext : IDisposable
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

        public IQueryable<Subscriber> Subscribers
        {
            get { return context.Subscribers.AsNoTracking(); }
        }

        public async Task SaveSubscriberAsync(Subscriber subscriber)
        {
            Subscriber entry = await context.Subscribers.Where(s => s.EMail == subscriber.EMail && s.Id != subscriber.Id).FirstOrDefaultAsync();
            if (entry != null)
            {
                entry.IsActive = true;
            }
            else
            {
                Subscriber forChange = await context.Subscribers.FindAsync(subscriber.Id);
                if (forChange == null)
                {
                    subscriber.Id = Guid.NewGuid();
                    subscriber.DateCreate = DateTime.Now;
                    subscriber.IsActive = true;
                    context.Subscribers.Add(subscriber);
                }
                else
                {
                    forChange.IsActive = subscriber.IsActive;
                    forChange.EMail = subscriber.EMail;
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteSubscriberAsync(Guid id)
        {
            Subscriber forDelete = await context.Subscribers.FindAsync(id);
            if (forDelete != null)
            {
                context.Subscribers.Remove(forDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task UnSubscribeAsync(Guid id)
        {
            Subscriber forChange = await context.Subscribers.FindAsync(id);
            if (forChange != null)
            {
                forChange.IsActive = false;
                await context.SaveChangesAsync();
            }
        }
    }
}

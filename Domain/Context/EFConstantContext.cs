using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class EFConstantContext : IDisposable
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

        public IQueryable<Constant> Constants
        {
            get { return context.Constants.AsNoTracking(); }
        }

        public async Task SaveConstantAsync(Constant constant)
        {
            if (constant.Id == Guid.Empty)
            {
                constant.Id = Guid.NewGuid();
                context.Constants.Add(constant);
            }
            else
            {
                Constant forChange = await context.Constants.FindAsync(constant.Id);
                if (forChange != null)
                {
                    forChange.Name = constant.Name;
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteConstantAsync(Guid constantId)
        {
            Constant forDelete = await context.Constants.FindAsync(constantId);
            if (forDelete != null)
            {
                context.Constants.Remove(forDelete);
            }
            await context.SaveChangesAsync();
        }
    }
}

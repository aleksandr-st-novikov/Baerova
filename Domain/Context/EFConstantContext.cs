using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        /// <summary>
        /// Сохранение константы
        /// </summary>
        /// <param name="constant"></param>
        /// <returns></returns>
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
                    forChange.Value = constant.Value;
                }
            }
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление константы
        /// </summary>
        /// <param name="constantId"></param>
        /// <returns></returns>
        public async Task DeleteConstantAsync(Guid constantId)
        {
            Constant forDelete = await context.Constants.FindAsync(constantId);
            if (forDelete != null)
            {
                context.Constants.Remove(forDelete);
            }
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Получение значения константы по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> GetConstantAsync(string name)
        {
            Constant constant = await context.Constants.Where(c => c.Name == name).FirstOrDefaultAsync();
            return constant == null ? null : constant.Value;
        }
    }
}

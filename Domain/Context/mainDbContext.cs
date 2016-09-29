using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain.Context
{
    public class mainDbContext : DbContext
    {
        public mainDbContext()
            : base("mainDbContext")
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<CountView> CountViews { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<MenuSet> MenuSets { get; set; }
        public DbSet<MailArticle> MailArticles { get; set; }
        //public DbSet<AspNetUser> AspNetUsers { get; set; }
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<mainDbContext>
    {
        //protected override void Seed(mainDbContext context)
        //{
        //    var articles = new List<Article>
        //    {
        //        new Article {Id = Guid.NewGuid(), DateCreate = DateTime.Now },
        //        new Article {Id = Guid.NewGuid(), DateCreate = DateTime.Now }
        //    };
        //    context.Configuration.AutoDetectChangesEnabled = false;
        //    articles.ForEach(a => context.Articles.Add(a));
        //    context.ChangeTracker.DetectChanges();
        //    context.SaveChanges();

        //    var subs = new List<Subscriber>
        //    {
        //        new Subscriber {Id = Guid.NewGuid(), EMail = "sfsdfg@sdgf.com" },
        //        new Subscriber {Id = Guid.NewGuid(), EMail = "fgsdfg@gdfgh.com" }
        //    };
        //    context.Configuration.AutoDetectChangesEnabled = false;
        //    subs.ForEach(s => context.Subscribers.Add(s));
        //    context.ChangeTracker.DetectChanges();
        //    context.SaveChanges();
        //}

    }
}

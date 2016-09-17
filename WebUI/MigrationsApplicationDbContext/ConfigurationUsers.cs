namespace WebUI.MigrationsApplicationDbContext
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationUsers : DbMigrationsConfiguration<WebUI.Models.ApplicationDbContext>
    {
        public ConfigurationUsers()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"MigrationsApplicationDbContext";
        }

        protected override void Seed(WebUI.Models.ApplicationDbContext context)
        {
            //var passwordHash = new PasswordHasher();
            //string password = passwordHash.HashPassword("Az_1234");
            //context.Users.AddOrUpdate(
            //    new Models.ApplicationUser { UserName = "user", PasswordHash = password, LockoutEnabled = false}
            //    );

            context.Roles.AddOrUpdate(
                new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Администратор" },
                new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Редактор" }
                );
            context.SaveChanges();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindByName("user");
            if (user == null)
            {
                var newUser = new ApplicationUser()
                {
                    UserName = "user"
                };
                userManager.Create(newUser, "Az_1234");
                userManager.SetLockoutEnabled(newUser.Id, false);
                userManager.AddToRole(newUser.Id, "Администратор");
            }
        }

    }
}

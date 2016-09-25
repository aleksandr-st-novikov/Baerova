namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //internal sealed class Configuration : DbMigrationsConfiguration<WebUI.Models.ApplicationDbContext>
    internal sealed class ConfigurationMain : DbMigrationsConfiguration<Domain.Context.mainDbContext>
    {
        public ConfigurationMain()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Domain.Context.mainDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.MenuSets.AddOrUpdate(
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Id,
                    Group = Domain.Entities.Groups.Главная, IsActive = true, Order = 1, Name = "О компании",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Выгодный старт") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Выгодный старт").Id,
                    Group = Domain.Entities.Groups.Дистрибьюторам_TianDe, IsActive = true, Order = 1, Name = "Выгодный старт",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Стабильные продажи") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Стабильные продажи").Id,
                    Group = Domain.Entities.Groups.Дистрибьюторам_TianDe, IsActive = true, Order = 2, Name = "Стабильные продажи",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Маркетинг-план компании") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Маркетинг-план компании").Id,
                    Group = Domain.Entities.Groups.Бизнес_с_TianDe, IsActive = true, Order = 1, Name = "Маркетинг-план компании",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Звездный маркетинг") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Звездный маркетинг").Id,
                    Group = Domain.Entities.Groups.Бизнес_с_TianDe, IsActive = true, Order = 2, Name = "Звездный маркетинг",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Премиум-курс") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Премиум-курс").Id,
                    Group = Domain.Entities.Groups.Бизнес_с_TianDe, IsActive = true, Order = 3, Name = "Премиум-курс",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Магистр TianDe") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Магистр TianDe").Id,
                    Group = Domain.Entities.Groups.Бизнес_с_TianDe, IsActive = true, Order = 4, Name = "Магистр TianDe",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Архитектор. Создай свой доход") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Архитектор. Создай свой доход").Id,
                    Group = Domain.Entities.Groups.Бизнес_с_TianDe, IsActive = true, Order = 5, Name = "Архитектор. Создай свой доход",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О компании") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О компании").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "О нас") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "О нас").Id,
                    Group = Domain.Entities.Groups.Информация, IsActive = true, Order = 1, Name = "О нас",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "О нас") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "О нас").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Магазины TianDe") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Магазины TianDe").Id,
                    Group = Domain.Entities.Groups.Информация, IsActive = true, Order = 2, Name = "Магазины TianDe",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "Магазины TianDe") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "Магазины TianDe").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Лидеры команды") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Лидеры команды").Id,
                    Group = Domain.Entities.Groups.Информация, IsActive = true, Order = 3, Name = "Лидеры команды",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "Лидеры команды") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "Лидеры команды").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Открой свой магазин TianDe - Сервисный центр") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Открой свой магазин TianDe - Сервисный центр").Id,
                    Group = Domain.Entities.Groups.Предпринимателям, IsActive = true, Order = 1, Name = "Открой свой магазин TianDe - Сервисный центр",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "Открой свой магазин TianDe - Сервисный центр") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "Открой свой магазин TianDe - Сервисный центр").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "Положение о Сервисных центрах") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "Положение о Сервисных центрах").Id,
                    Group = Domain.Entities.Groups.Предпринимателям, IsActive = true, Order = 2, Name = "Положение о Сервисных центрах",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "Положение о Сервисных центрах") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "Положение о Сервисных центрах").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "StartUp для начинающих - специальная программа") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "StartUp для начинающих - специальная программа").Id,
                    Group = Domain.Entities.Groups.Предпринимателям, IsActive = true, Order = 3, Name = "StartUp для начинающих - специальная программа",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "StartUp для начинающих - специальная программа") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "StartUp для начинающих - специальная программа").Link
                }
                );
        }
    }
}

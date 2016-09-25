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
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Id,
                    Group = Domain.Entities.Groups.�������, IsActive = true, Order = 1, Name = "� ��������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "�������� �����") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "�������� �����").Id,
                    Group = Domain.Entities.Groups.��������������_TianDe, IsActive = true, Order = 1, Name = "�������� �����",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "���������� �������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "���������� �������").Id,
                    Group = Domain.Entities.Groups.��������������_TianDe, IsActive = true, Order = 2, Name = "���������� �������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "���������-���� ��������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "���������-���� ��������").Id,
                    Group = Domain.Entities.Groups.������_�_TianDe, IsActive = true, Order = 1, Name = "���������-���� ��������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "�������� ���������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "�������� ���������").Id,
                    Group = Domain.Entities.Groups.������_�_TianDe, IsActive = true, Order = 2, Name = "�������� ���������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "�������-����") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "�������-����").Id,
                    Group = Domain.Entities.Groups.������_�_TianDe, IsActive = true, Order = 3, Name = "�������-����",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "������� TianDe") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "������� TianDe").Id,
                    Group = Domain.Entities.Groups.������_�_TianDe, IsActive = true, Order = 4, Name = "������� TianDe",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "����������. ������ ���� �����") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "����������. ������ ���� �����").Id,
                    Group = Domain.Entities.Groups.������_�_TianDe, IsActive = true, Order = 5, Name = "����������. ������ ���� �����",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ��������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ��������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "� ���") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "� ���").Id,
                    Group = Domain.Entities.Groups.����������, IsActive = true, Order = 1, Name = "� ���",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "� ���") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "� ���").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "�������� TianDe") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "�������� TianDe").Id,
                    Group = Domain.Entities.Groups.����������, IsActive = true, Order = 2, Name = "�������� TianDe",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "�������� TianDe") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "�������� TianDe").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "������ �������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "������ �������").Id,
                    Group = Domain.Entities.Groups.����������, IsActive = true, Order = 3, Name = "������ �������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "������ �������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "������ �������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "������ ���� ������� TianDe - ��������� �����") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "������ ���� ������� TianDe - ��������� �����").Id,
                    Group = Domain.Entities.Groups.����������������, IsActive = true, Order = 1, Name = "������ ���� ������� TianDe - ��������� �����",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "������ ���� ������� TianDe - ��������� �����") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "������ ���� ������� TianDe - ��������� �����").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "��������� � ��������� �������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "��������� � ��������� �������").Id,
                    Group = Domain.Entities.Groups.����������������, IsActive = true, Order = 2, Name = "��������� � ��������� �������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "��������� � ��������� �������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "��������� � ��������� �������").Link
                },
                new Domain.Entities.MenuSet { Id = context.MenuSets.FirstOrDefault(m => m.Name == "StartUp ��� ���������� - ����������� ���������") == null ? 
                    Guid.NewGuid() : context.MenuSets.FirstOrDefault(m => m.Name == "StartUp ��� ���������� - ����������� ���������").Id,
                    Group = Domain.Entities.Groups.����������������, IsActive = true, Order = 3, Name = "StartUp ��� ���������� - ����������� ���������",
                    Link = context.MenuSets.FirstOrDefault(m => m.Name == "StartUp ��� ���������� - ����������� ���������") == null ?
                    "/" : context.MenuSets.FirstOrDefault(m => m.Name == "StartUp ��� ���������� - ����������� ���������").Link
                }
                );
        }
    }
}

namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Partner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Surname = c.String(nullable: false, maxLength: 150),
                        Name = c.String(nullable: false, maxLength: 100),
                        Patronymic = c.String(maxLength: 100),
                        DOB = c.DateTime(nullable: false),
                        EMail = c.String(),
                        Phone = c.String(maxLength: 30),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Articles", "DatePublish", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Articles", "TextMain", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "TextMain", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "DatePublish", c => c.DateTime());
            DropTable("dbo.Partners");
        }
    }
}

namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Descr = c.String(),
                        PictShare = c.String(),
                        IsVisible = c.Boolean(nullable: false),
                        DatePublish = c.DateTime(nullable: false),
                        TextMain = c.String(),
                        TextArticle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EMail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subscribers");
            DropTable("dbo.Articles");
        }
    }
}

namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleRestriction : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "Title", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Articles", "Descr", c => c.String(maxLength: 500));
            AlterColumn("dbo.Articles", "PictShare", c => c.String(maxLength: 250));
            AlterColumn("dbo.Articles", "TextMain", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "TextArticle", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "Link", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "Link", c => c.String());
            AlterColumn("dbo.Articles", "TextArticle", c => c.String());
            AlterColumn("dbo.Articles", "TextMain", c => c.String());
            AlterColumn("dbo.Articles", "PictShare", c => c.String());
            AlterColumn("dbo.Articles", "Descr", c => c.String());
            AlterColumn("dbo.Articles", "Title", c => c.String());
        }
    }
}

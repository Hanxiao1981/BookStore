namespace Taihang.BookStore.Net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Img = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Author = c.String(maxLength: 50),
                        Describe = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Disable = c.Boolean(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BookCategory", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.BookCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Book", "CategoryID", "dbo.BookCategory");
            DropIndex("dbo.Book", new[] { "CategoryID" });
            DropTable("dbo.BookCategory");
            DropTable("dbo.Book");
        }
    }
}

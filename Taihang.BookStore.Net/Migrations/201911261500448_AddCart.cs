namespace Taihang.BookStore.Net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CartId = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .Index(t => t.BookID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItem", "BookID", "dbo.Book");
            DropIndex("dbo.CartItem", new[] { "BookID" });
            DropTable("dbo.CartItem");
        }
    }
}

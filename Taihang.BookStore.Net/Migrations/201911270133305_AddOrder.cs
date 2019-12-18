namespace Taihang.BookStore.Net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 50),
                        ReciverName = c.String(maxLength: 50),
                        ReciverPhone = c.String(maxLength: 50),
                        PostAddr = c.String(maxLength: 100),
                        TotalSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderDate = c.DateTime(nullable: false),
                        StateDescribe = c.String(maxLength: 50),
                        OrderState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Img = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Author = c.String(maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "BookID", "dbo.Book");
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropIndex("dbo.OrderItem", new[] { "BookID" });
            DropTable("dbo.OrderItem");
            DropTable("dbo.Order");
        }
    }
}

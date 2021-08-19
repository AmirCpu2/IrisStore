namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeItemCollactionConfig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductItems", "ItemType_Id", "dbo.ItemType");
            DropForeignKey("dbo.ProductItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ProductItems", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductItems", new[] { "ItemType_Id" });
            DropTable("dbo.ProductItems");
            CreateTable(
                "dbo.ProductItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.ProductId })
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ItemType_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.ItemId, t.ProductId });
            
            DropForeignKey("dbo.ProductItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductItems", "ItemId", "dbo.Items");
            DropTable("dbo.ProductItems");
            CreateIndex("dbo.ProductItems", "ItemType_Id");
            AddForeignKey("dbo.ProductItems", "ProductId", "dbo.Products", "Id");
            AddForeignKey("dbo.ProductItems", "ItemId", "dbo.Items", "Id");
            AddForeignKey("dbo.ProductItems", "ItemType_Id", "dbo.ItemType", "Id");
        }
    }
}

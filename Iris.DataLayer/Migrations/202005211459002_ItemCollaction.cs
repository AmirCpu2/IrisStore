namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemCollaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductItems",
                c => new
                    {
                        ItemsId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ItemTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemsId, t.ProductId })
                .ForeignKey("dbo.ItemType", t => t.ItemTypeId)
                .ForeignKey("dbo.Items", t => t.ItemsId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ItemsId)
                .Index(t => t.ProductId)
                .Index(t => t.ItemTypeId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameFa = c.String(nullable: false, maxLength: 256),
                        NameEn = c.String(nullable: false, maxLength: 256),
                        ItemTypeId = c.Int(nullable: false),
                        DisplayPriority = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeId)
                .Index(t => t.ItemTypeId);
            
            CreateTable(
                "dbo.ItemType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameFa = c.String(nullable: false, maxLength: 256),
                        NameEn = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductItems", "ItemsId", "dbo.Items");
            DropForeignKey("dbo.ProductItems", "ItemTypeId", "dbo.ItemType");
            DropForeignKey("dbo.Items", "ItemTypeId", "dbo.ItemType");
            DropIndex("dbo.Items", new[] { "ItemTypeId" });
            DropIndex("dbo.ProductItems", new[] { "ItemTypeId" });
            DropIndex("dbo.ProductItems", new[] { "ProductId" });
            DropIndex("dbo.ProductItems", new[] { "ItemsId" });
            DropTable("dbo.ItemType");
            DropTable("dbo.Items");
            DropTable("dbo.ProductItems");
        }
    }
}

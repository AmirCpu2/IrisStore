namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePropertyFildsAndRelations : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductProperty", newName: "ProductProperties");
            DropForeignKey("dbo.ProductProperty", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductProperty", "PropertyId", "dbo.Properties");
            DropIndex("dbo.ProductProperties", new[] { "ProductId" });
            DropIndex("dbo.ProductProperties", new[] { "PropertyId" });
            DropPrimaryKey("dbo.ProductProperties");
            AddColumn("dbo.ProductProperties", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ProductProperties", "Value", c => c.String(nullable: false));
            AddColumn("dbo.ProductProperties", "ShowInIntro", c => c.Boolean());
            AddColumn("dbo.ProductProperties", "DisplayOrder", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProductProperties", "Id");
            CreateIndex("dbo.ProductProperties", "PropertyId");
            CreateIndex("dbo.ProductProperties", "ProductId");
            AddForeignKey("dbo.ProductProperties", "ProductId", "dbo.Products", "Id");
            AddForeignKey("dbo.ProductProperties", "PropertyId", "dbo.Properties", "Id");
            DropColumn("dbo.Properties", "SortingPriority");
            DropColumn("dbo.Properties", "ShowInIntro");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Properties", "ShowInIntro", c => c.Boolean(nullable: false));
            AddColumn("dbo.Properties", "SortingPriority", c => c.Int());
            DropForeignKey("dbo.ProductProperties", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.ProductProperties", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductProperties", new[] { "ProductId" });
            DropIndex("dbo.ProductProperties", new[] { "PropertyId" });
            DropPrimaryKey("dbo.ProductProperties");
            DropColumn("dbo.ProductProperties", "DisplayOrder");
            DropColumn("dbo.ProductProperties", "ShowInIntro");
            DropColumn("dbo.ProductProperties", "Value");
            DropColumn("dbo.ProductProperties", "Id");
            AddPrimaryKey("dbo.ProductProperties", new[] { "ProductId", "PropertyId" });
            CreateIndex("dbo.ProductProperties", "PropertyId");
            CreateIndex("dbo.ProductProperties", "ProductId");
            AddForeignKey("dbo.ProductProperty", "PropertyId", "dbo.Properties", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductProperty", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.ProductProperties", newName: "ProductProperty");
        }
    }
}

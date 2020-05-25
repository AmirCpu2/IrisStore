namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProductItems : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProductItems", new[] { "ItemTypeId" });
            RenameColumn(table: "dbo.ProductItems", name: "ItemsId", newName: "ItemId");
            RenameColumn(table: "dbo.ProductItems", name: "ItemTypeId", newName: "ItemType_Id");
            RenameIndex(table: "dbo.ProductItems", name: "IX_ItemsId", newName: "IX_ItemId");
            AlterColumn("dbo.ProductItems", "ItemType_Id", c => c.Int());
            CreateIndex("dbo.ProductItems", "ItemType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductItems", new[] { "ItemType_Id" });
            AlterColumn("dbo.ProductItems", "ItemType_Id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.ProductItems", name: "IX_ItemId", newName: "IX_ItemsId");
            RenameColumn(table: "dbo.ProductItems", name: "ItemType_Id", newName: "ItemTypeId");
            RenameColumn(table: "dbo.ProductItems", name: "ItemId", newName: "ItemsId");
            CreateIndex("dbo.ProductItems", "ItemTypeId");
        }
    }
}

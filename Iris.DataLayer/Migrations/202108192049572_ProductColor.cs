namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductColor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductColor");
        }
    }
}

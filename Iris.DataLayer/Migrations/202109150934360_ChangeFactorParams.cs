namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFactorParams : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Factors", "PublicId", c => c.Guid(nullable: false, identity: true));
            AddColumn("dbo.Factors", "PostalCode", c => c.String());
            AddColumn("dbo.Factors", "RefId", c => c.String());
            CreateIndex("dbo.Factors", "PublicId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Factors", new[] { "PublicId" });
            DropColumn("dbo.Factors", "RefId");
            DropColumn("dbo.Factors", "PostalCode");
            DropColumn("dbo.Factors", "PublicId");
        }
    }
}

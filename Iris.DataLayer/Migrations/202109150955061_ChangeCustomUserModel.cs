namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCustomUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PostalCode", c => c.String());
            AddColumn("dbo.Users", "LatLong", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LatLong");
            DropColumn("dbo.Users", "PostalCode");
        }
    }
}

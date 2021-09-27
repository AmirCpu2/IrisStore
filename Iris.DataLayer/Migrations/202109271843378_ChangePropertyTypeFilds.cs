namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePropertyTypeFilds : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PropertyTypes", "IsEdited", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PropertyTypes", "EditDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PropertyTypes", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PropertyTypes", "IsEdited", c => c.String(nullable: false, maxLength: 10, fixedLength: true));
        }
    }
}

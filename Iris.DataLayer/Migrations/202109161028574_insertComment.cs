namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class insertComment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ColorProducts", newName: "ColorProduct");
            DropForeignKey("dbo.PostComments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes");
            DropIndex("dbo.PostComments", new[] { "PostId" });
            RenameColumn(table: "dbo.ColorProduct", name: "Color_Id", newName: "ColorId");
            RenameColumn(table: "dbo.ColorProduct", name: "Product_Id", newName: "ProductId");
            RenameIndex(table: "dbo.ColorProduct", name: "IX_Color_Id", newName: "IX_ColorId");
            RenameIndex(table: "dbo.ColorProduct", name: "IX_Product_Id", newName: "IX_ProductId");
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                        RegesterDate = c.DateTime(),
                        status = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsEdited = c.Boolean(nullable: false),
                        TextEdit = c.String(),
                        ParentId = c.Int(nullable: false),
                        EditedDate = c.DateTime(),
                        IsQuestionAnswer = c.Boolean(nullable: false),
                        Post_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.ParentId)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.ProductQuestionAnswers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ParentId)
                .Index(t => t.Post_Id)
                .Index(t => t.Product_Id);
            
            AlterColumn("dbo.PropertyTypes", "IsEdited", c => c.String(nullable: false, maxLength: 10, fixedLength: true));
            AddForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes", "Id");
            DropTable("dbo.PostComments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId);
            
            DropForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes");
            DropForeignKey("dbo.Comments", "Id", "dbo.ProductQuestionAnswers");
            DropForeignKey("dbo.Comments", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "ParentId", "dbo.Comments");
            DropIndex("dbo.Comments", new[] { "Product_Id" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "ParentId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            AlterColumn("dbo.PropertyTypes", "IsEdited", c => c.String(nullable: false, maxLength: 10));
            DropTable("dbo.Comments");
            RenameIndex(table: "dbo.ColorProduct", name: "IX_ProductId", newName: "IX_Product_Id");
            RenameIndex(table: "dbo.ColorProduct", name: "IX_ColorId", newName: "IX_Color_Id");
            RenameColumn(table: "dbo.ColorProduct", name: "ProductId", newName: "Product_Id");
            RenameColumn(table: "dbo.ColorProduct", name: "ColorId", newName: "Color_Id");
            CreateIndex("dbo.PostComments", "PostId");
            AddForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostComments", "PostId", "dbo.Posts", "Id");
            RenameTable(name: "dbo.ColorProduct", newName: "ColorProducts");
        }
    }
}

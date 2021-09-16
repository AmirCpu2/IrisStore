namespace Iris.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PostTags", newName: "TagPosts");
            DropPrimaryKey("dbo.TagPosts");
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(nullable: false, maxLength: 150),
                        NameFA = c.String(nullable: false, maxLength: 150),
                        Code = c.String(nullable: false, maxLength: 50),
                        IsDetele = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserFavoritePosts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.PostComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.UserFavoriteProducts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProductId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductQuestionAnswers",
                c => new
                    {
                        CommentId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(nullable: false, maxLength: 250),
                        NameFA = c.String(nullable: false, maxLength: 250),
                        SortingPriority = c.Int(),
                        PropertyTypeId = c.Int(nullable: false),
                        ShowInIntro = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PropertyTypes", t => t.PropertyTypeId, cascadeDelete: true)
                .Index(t => t.PropertyTypeId);
            
            CreateTable(
                "dbo.PropertyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(nullable: false, maxLength: 150),
                        NameFA = c.String(nullable: false, maxLength: 150),
                        IsArchive = c.Boolean(nullable: false),
                        IsEdited = c.String(nullable: false, maxLength: 10),
                        EditDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ColorProducts",
                c => new
                    {
                        Color_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Color_Id, t.Product_Id })
                .ForeignKey("dbo.Colors", t => t.Color_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Color_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.ProductProperty",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        PropertyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.PropertyId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Properties", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.PropertyId);
            
            AddColumn("dbo.ProductPrices", "Product_Id", c => c.Int());
            AddPrimaryKey("dbo.TagPosts", new[] { "Tag_Id", "Post_Id" });
            CreateIndex("dbo.ProductPrices", "Product_Id");
            AddForeignKey("dbo.ProductPrices", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFavoriteProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductProperty", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.ProductProperty", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes");
            DropForeignKey("dbo.ProductQuestionAnswers", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductPrices", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.UserFavoriteProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserFavoritePosts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserFavoritePosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostComments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.ColorProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ColorProducts", "Color_Id", "dbo.Colors");
            DropIndex("dbo.ProductProperty", new[] { "PropertyId" });
            DropIndex("dbo.ProductProperty", new[] { "ProductId" });
            DropIndex("dbo.ColorProducts", new[] { "Product_Id" });
            DropIndex("dbo.ColorProducts", new[] { "Color_Id" });
            DropIndex("dbo.Properties", new[] { "PropertyTypeId" });
            DropIndex("dbo.ProductQuestionAnswers", new[] { "ProductId" });
            DropIndex("dbo.ProductPrices", new[] { "Product_Id" });
            DropIndex("dbo.UserFavoriteProducts", new[] { "ProductId" });
            DropIndex("dbo.UserFavoriteProducts", new[] { "UserId" });
            DropIndex("dbo.PostComments", new[] { "PostId" });
            DropIndex("dbo.UserFavoritePosts", new[] { "PostId" });
            DropIndex("dbo.UserFavoritePosts", new[] { "UserId" });
            DropPrimaryKey("dbo.TagPosts");
            DropColumn("dbo.ProductPrices", "Product_Id");
            DropTable("dbo.ProductProperty");
            DropTable("dbo.ColorProducts");
            DropTable("dbo.PropertyTypes");
            DropTable("dbo.Properties");
            DropTable("dbo.ProductQuestionAnswers");
            DropTable("dbo.UserFavoriteProducts");
            DropTable("dbo.PostComments");
            DropTable("dbo.UserFavoritePosts");
            DropTable("dbo.Colors");
            AddPrimaryKey("dbo.TagPosts", new[] { "Post_Id", "Tag_Id" });
            RenameTable(name: "dbo.TagPosts", newName: "PostTags");
        }
    }
}

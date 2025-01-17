﻿using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class PostConfig : EntityTypeConfiguration<Post>
    {
        public PostConfig()
        {
            Property(post => post.Title).HasMaxLength(500);

            Property(post => post.Body).IsMaxLength();

            Property(post => post.MetaDescription).HasMaxLength(400);

            Property(post => post.SlugUrl).HasMaxLength(300);

            //this.HasMany(e => e.PostComments)
            //    .WithRequired(e => e.Post)
            //    .WillCascadeOnDelete(false);

            this.HasMany(e => e.UserFavoritePosts)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);
        }
    }
}

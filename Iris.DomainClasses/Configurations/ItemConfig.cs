﻿using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ItemConfig : EntityTypeConfiguration<Item>
    {
        public ItemConfig()
        {
            HasMany(e => e.ProductItems)
            .WithRequired(e => e.Item)
            .HasForeignKey(e => e.ItemsId)
            .WillCascadeOnDelete(false);

            Property(entity => entity.NameEn).HasMaxLength(256);
            Property(entity => entity.NameEn).IsRequired();

            Property(entity => entity.NameFa).HasMaxLength(256);
            Property(entity => entity.NameFa).IsRequired();
        }
    }
}
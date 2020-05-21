using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ItemTypeConfig : EntityTypeConfiguration<ItemType>
    {
        public ItemTypeConfig() 
        {
            HasMany(e => e.Items)
            .WithRequired(e => e.ItemType)
            .WillCascadeOnDelete(false);

            HasMany(e => e.ProductItems)
            .WithRequired(e => e.ItemType)
            .WillCascadeOnDelete(false);

            Property(entity => entity.NameEn).HasMaxLength(256);
            Property(entity => entity.NameEn).IsRequired();

            Property(entity => entity.NameFa).HasMaxLength(256);
            Property(entity => entity.NameFa).IsRequired();
        }
    }
}

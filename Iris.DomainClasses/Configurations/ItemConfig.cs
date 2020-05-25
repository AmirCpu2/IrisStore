using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ItemConfig : EntityTypeConfiguration<Item>
    {
        public ItemConfig()
        {
            HasMany(e => e.Products)
            .WithMany(e => e.Items)
            .Map(m => m.ToTable("ProductItems").MapLeftKey("ItemId").MapRightKey("ProductId"));

            Property(entity => entity.NameEn).HasMaxLength(256);
            Property(entity => entity.NameEn).IsRequired();

            Property(entity => entity.NameFa).HasMaxLength(256);
            Property(entity => entity.NameFa).IsRequired();
        }
    }
}

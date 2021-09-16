using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class ColorConfig : EntityTypeConfiguration<Color>
    {
        public ColorConfig()
        {
            this.HasMany(e => e.Products)
                .WithMany(e => e.Colors)
                .Map(m => m.ToTable("ColorProduct").MapLeftKey("ColorId").MapRightKey("ProductId"));
        }
    }
}

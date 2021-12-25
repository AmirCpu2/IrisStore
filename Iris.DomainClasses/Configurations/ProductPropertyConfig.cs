using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class ProductPropertyConfig : EntityTypeConfiguration<ProductProperty>
    {
        public ProductPropertyConfig()
        {

            HasRequired(e => e.Property).
                WithMany(q => q.ProductProperties)
                .HasForeignKey(p => p.PropertyId);
            

            //HasMany(e => e.ProductProperties)
            //    .WithRequired(e => e.Property)
            //    .HasForeignKey(e => e.PropertysId)
            //    .WillCascadeOnDelete(false);
        }
    }
}

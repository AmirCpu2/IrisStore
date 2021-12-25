using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class PropertyConfig : EntityTypeConfiguration<Property>
    {
        public PropertyConfig()
        {
            //HasMany(e => e.ProductProperties)
            //    .WithRequired(e => e.Property)
            //    .HasForeignKey(e => e.PropertysId)
            //    .WillCascadeOnDelete(false);
        }
    }
}

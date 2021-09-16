using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class PropertyTypeConfig : EntityTypeConfiguration<PropertyType>
    {
        public PropertyTypeConfig()
        {
            Property(e => e.IsEdited)
                .IsFixedLength();

            HasMany(e => e.Properties)
                .WithRequired(e => e.PropertyType)
                .WillCascadeOnDelete(false);
        }
    }
}

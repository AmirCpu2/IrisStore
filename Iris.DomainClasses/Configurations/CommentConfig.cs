using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class CommentConfig : EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            HasMany(e => e.Comment1)
                .WithRequired(e => e.Comment2)
                .HasForeignKey(e => e.ParentId);
        }
    }
}

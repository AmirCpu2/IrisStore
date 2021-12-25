using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            HasKey(x => x.Id)
            .Property(q => q.Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            ;




        }
    }
}

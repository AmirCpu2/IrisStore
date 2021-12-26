using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class BidHistoryConfig : EntityTypeConfiguration<BidHistory>
    {
        public BidHistoryConfig()
        {
            HasRequired(bid => bid.User)
                .WithMany(user => user.BidHistory)
                .HasForeignKey(bid => bid.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}

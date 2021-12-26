using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class CoinFactorConfig : EntityTypeConfiguration<CoinFactor>
    {
        public CoinFactorConfig()
        {
            HasRequired(bid => bid.User)
                .WithMany(user => user.CoinFactor)
                .HasForeignKey(bid => bid.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}

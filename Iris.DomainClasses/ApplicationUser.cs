using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DomainClasses
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

        public ApplicationUser()
        {
            BidHistory = new HashSet<BidHistory>();
            CoinFactor = new HashSet<CoinFactor>();
        }

        public virtual ICollection<Factor> Factors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoritePost> UserFavoritePosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoriteProduct> UserFavoriteProducts { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Address { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string LatLong { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual string ThumbnailUrl { get; set; }

        public virtual int? BidCount { get; set; }

        public virtual DateTime? LastModifiedBidDate { get; set; }

        public int? BidCountUse { get; set; }

        public virtual ICollection<BidHistory> BidHistory { get; set; }
        public virtual ICollection<CoinFactor> CoinFactor { get; set; }

    }
}
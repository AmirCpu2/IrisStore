using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Iris.DomainClasses
{
    public enum ProductStatus
    {
        [Description("موجود")]
        Available,
        [Description("ناموجود")]
        NotAvailable,
        [Description("به زودی")]
        ComingSoon,
    }

    public class Product : BaseEntity
    {
        public Product()
        {
            Prices = new HashSet<ProductPrice>();
            Discounts = new HashSet<ProductDiscount>();
            Categories = new HashSet<Category>();
            Images = new HashSet<ProductImage>();
            //ProductQuestionAnswers = new HashSet<ProductQuestionAnswer>();
            UserFavoriteProducts = new HashSet<UserFavoriteProduct>();
            Colors = new HashSet<Color>();
            Comments = new HashSet<Comment>();
            ProductProperties = new HashSet<ProductProperty>();
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public int PostedByUserId { get; set; }
        public ApplicationUser PostedByUser { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string MetaDescription { get; set; }
        public string SlugUrl { get; set; }
        public int ViewNumber { get; set; }
        public int Count { get; set; }

        public double? TotalRating { get; set; }
        public int? TotalRaters { get; set; }
        public double? AverageRating { get; set; }

        public ProductStatus ProductStatus { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ProductPrice> Prices { get; set; }
        public virtual ICollection<ProductDiscount> Discounts { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductQuestionAnswer> ProductQuestionAnswers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoriteProduct> UserFavoriteProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Color> Colors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductProperty> ProductProperties { get; set; }
    }
}

using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ViewModels
{
    public class UserFavoriteProductViewModel : IHaveCustomMappings
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime DateTime { get; set; }

        public virtual FavoriteProductViewModel Product { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserFavoriteProduct, UserFavoriteProductViewModel>()
                .ForMember(Favorite => Favorite.Product, opt => opt.MapFrom(q => Mapper.Map<FavoriteProductViewModel>(q.Product)));
        }

    }

    public class FavoriteProductViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public DateTime PostedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Discount { get; set; }
        public IList<ProductDiscountViewModel> Discounts { get; set; }
        public string SlugUrl { get; set; }
        public ProductStatus ProductStatus { get; set; }


        #region AngolarProprty

        public ProductDiscountViewModel CurrentDiscounts => Discounts?.FirstOrDefault(q => q.EndDate > DateTime.Now);

        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentDiscount => CurrentDiscounts?.Discount ?? 0;

        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentPrice => (Price - ((Price * CurrentDiscount) / 100));

        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentDiscountFee => (((Price * CurrentDiscount) / 100));
        #endregion


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, FavoriteProductViewModel>()
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(s => s.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Price))

                .ForMember(d => d.Discounts,
                    opt => opt.MapFrom(s => (s.Discounts) ))

                .ForMember(productModel => productModel.Discount,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Discounts.OrderByDescending(discount => discount.StartDate)
                                    .Select(discount => discount.Discount)
                                    .FirstOrDefault()))

                .ForMember(productModel => productModel.ThumbnailUrl,
                    opt => opt.MapFrom(product => product.Images.OrderBy(image => image.Order).FirstOrDefault().ThumbnailUrl))


                .ForMember(product => product.SlugUrl, opt => opt.MapFrom(productModel => productModel.SlugUrl))
            ;

        }
    }

    
}

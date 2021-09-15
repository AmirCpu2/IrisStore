using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using Utilities;

namespace Iris.ViewModels
{
    public class ProductOrderViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Discount { get; set; }
        public int Count { get; set; }
        public IList<ProductDiscountViewModel> Discounts { get; set; }
        public string ProductColor { get; set; }
        public string Sellers { get; set; }
        public string Brand { get; set; }
        public IList<ProductPageImageViewModel> Images { get; set; }
        public string Categories { get; set; }
        public string SlugUrl { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductOrderViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(s => s.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Price))

                .ForMember(productModel => productModel.Discount,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Discounts.OrderByDescending(discount => discount.StartDate)
                                    .Select(discount => discount.Discount)
                                    .FirstOrDefault()))

                .ForMember(productModel => productModel.Images,
                    opt => opt.MapFrom(product => product.Images.OrderBy(image => image.Order)))


                .ForMember(productModel => productModel.ProductColor, opt => opt.MapFrom(q => q.Items
                    .Where(p => p.ItemType.NameEn.Equals(Enums.ItemType.ProductColor.ToString())).Select(pi => pi.NameFa).FirstOrDefault()))

                .ForMember(productModel => productModel.Sellers, opt => opt.MapFrom(q => q.Items
                    .Where(p => p.ItemType.NameEn.Equals(Enums.ItemType.Seller.ToString())).Select(pi => pi.NameFa).FirstOrDefault()))

                .ForMember(productModel => productModel.Brand, opt => opt.MapFrom(q => q.Items
                    .Where(p => p.ItemType.NameEn.Equals(Enums.ItemType.Brand.ToString())).Select(pi => pi.NameFa).FirstOrDefault()))

                .ForMember(p => p.Categories, opt => opt.MapFrom(x => x.Categories.Select(c => c.Name).FirstOrDefault()))

                .ForMember(product => product.SlugUrl, opt => opt.MapFrom(productModel => productModel.SlugUrl));
        }

        #region Calculator Properties
        /// <summary>
        /// Calculator Discounts
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscount { get { return (Price - ((Price * Discount) / 100)); } }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscountFee { get { return (((Price * Discount) / 100)); } }
        #endregion


        #region AngolarProprty

        public ProductDiscountViewModel CurrentDiscounts => Discounts?.FirstOrDefault(q => q.EndDate > DateTime.Now);
        
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentDiscount => CurrentDiscounts?.Discount ?? 0;

        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentPrice => (Price - ((Price * CurrentDiscount) / 100));

        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CurrentDiscountFee => (((Price * CurrentDiscount) / 100));
        #endregion
    }

    public class ProductDiscountViewModel : IHaveCustomMappings
    {
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductDiscount, ProductDiscountViewModel>()
               .ForMember(discountModel => discountModel.StartDate, opt => opt.MapFrom(productdiscount => productdiscount.StartDate))
                           .ForMember(discountModel => discountModel.EndDate, opt => opt.MapFrom(productdiscount => productdiscount.EndDate));

            configuration.CreateMap<ProductDiscountViewModel, ProductDiscount>()
                .ForMember(discount => discount.Product, opt => opt.Ignore());
        }

    }
}

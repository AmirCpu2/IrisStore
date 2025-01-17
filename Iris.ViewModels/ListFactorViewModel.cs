﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.DomainClasses;
using AutoMapper;
using AutoMapperContracts;

namespace Iris.ViewModels
{
    public class ListFactorViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام را وارد کنید")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا آدرس را وارد کنید")]
        public string Address { get; set; }
        public Guid PublicId { get; set; }
        public string PostalCode { get; set; }
        public string RefId { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        [Required]
        public FactorStatus Status { get; set; } = FactorStatus.Paying;
        public IList<ListFactorProductViewModel> Products { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal TotalPrice => Products.Select(q => q.CalcDiscount * q.Count).Sum();
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal TotalPriceNormal => Products.Select(q => q.Price * q.Count).Sum();


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Factor, ListFactorViewModel>()
                .ForMember(Factor => Factor.Products, opt => opt.MapFrom(VMFactor => VMFactor.Products.Select(Mapper.Map<FactorProduct, ListFactorProductViewModel>)  ));
        }

    }

    public class ListFactorProductViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; } = 0;
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        
        #region Calculator Properties
        /// <summary>
        /// Calculator Discounts
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscount { get { return (Price - ((Price * Discount) / 100)); } }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscountFee { get { return (((Price * Discount) / 100)); } }
        #endregion


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<FactorProduct, ListFactorProductViewModel>()
                .ForMember(q=>q.ThumbnailUrl, op=>op.MapFrom(p=>p.Product.Images.FirstOrDefault().ThumbnailUrl))
                .ForMember(q=>q.Title, op=>op.MapFrom(p=>p.Product.Title))
                ;
        }
    }
}

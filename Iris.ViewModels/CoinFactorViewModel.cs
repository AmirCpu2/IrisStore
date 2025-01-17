﻿using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Iris.ViewModels
{
    public class CoinFactorViewModel : IHaveCustomMappings
    {

        [DisplayName("شناسه یکتا")]
        public Guid Id { get; set; }

        [DisplayName("نوع بسته")]
        public int CoinPacketId { get; set; } = 0;

        [DisplayName("نوع بسته")]
        public Enums.CoinPacket CoinPacket => (Enums.CoinPacket)CoinPacketId;
        
        [DisplayName("نوع بسته")]
        public string CoinPacketFa => CoinPacketId > 0 ? CoinPacket.GetEnumDescription() : "";

        [DisplayName("تاریخ خرید")]
        public DateTime BuyDate { get; set; } = DateTime.Now;

        [DisplayName("تاریخ خرید")]
        public string BuyDateFa => PersianDateUtils.ToPersianDate(BuyDate);

        [DisplayName("وضعیت خرید")]
        public int StatusId { get; set; } = 0;

        [DisplayName("وضعیت خرید")]
        public Enums.CoinFactorStatus Status => (Enums.CoinFactorStatus)StatusId;
        
        [DisplayName("وضعیت خرید")]
        public string StatusFa => StatusId > -2 ? Status.GetEnumDescription() : "";

        [DisplayName("شناسه کابر")]
        public int UserId { get; set; }
        
        [DisplayName("شناسه پرداخت")]
        public string RefId { get; set; }

        [DisplayName("کاربر")]
        public virtual ApplicationUser User { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CoinFactor, CoinFactorViewModel>()
                .ForMember(x => x.CoinPacket, op=>op.MapFrom(q=> (Enums.CoinPacket) q.CoinPacketId));
                ;

            configuration.CreateMap<CoinFactorViewModel, CoinFactor>();
        }

    }
}

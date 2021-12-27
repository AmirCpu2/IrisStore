﻿using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iris.ViewModels
{
    public class AuctionItemViewModel : IHaveCustomMappings
    {
        public AuctionItemViewModel()
        {
            this.BidHistories = new List<BidHistory>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        [Required]
        public int PostedByUserId { get; set; }

        [Required]
        public DateTime CrateDate { get; set; } = DateTime.Now;

        public DateTime? EditDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1);

        [Required]
        public DateTime StopDate { get; set; } = DateTime.Now.AddDays(2);

        public string ImageName { get; set; }

        public string ImageAddress { get; set; }

        public HttpPostedFileBase ImageFileUpload { get; set; }

        public bool IsDelete { get; set; }

        public bool IsEdit { get; set; }

        [Required]
        public decimal MiniPrice { get; set; } = 50;

        [Required]
        public decimal MaxPrice { get; set; } = 200;

        public int? WinUserId { get; set; }


        public virtual ApplicationUser WinUser { get; set; }

        public virtual ApplicationUser PostedByUser { get; set; }

        public virtual ICollection<BidHistory> BidHistories { get; set; }



        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AuctionItem, AuctionItemViewModel>();
            configuration.CreateMap<AuctionItemViewModel, AuctionItem>();
        }
    }
}

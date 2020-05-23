using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ViewModels
{
    public class AddItemsViewModel : IHaveCustomMappings
    {
        public int? Id { get; set; }

        [DisplayName("نام اینگلیسی")]
        public string NameEn { get; set; }

        [DisplayName("نام فارسی")]
        public string NameFa { get; set; }

        [DisplayName("مشخصه گروه اصلی")]
        public int ItemTypeId { get; set; }

        [DisplayName("ترتیب")]
        public int? DisplayPriority { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AddItemsViewModel, Item>();

            configuration.CreateMap<Item, AddItemsViewModel>().ReverseMap();
        }
    }
}

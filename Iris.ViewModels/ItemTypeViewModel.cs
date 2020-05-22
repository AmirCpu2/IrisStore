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
    public class ItemTypeViewModel : IHaveCustomMappings
    {
        [DisplayName("مشخصه")]
        public int Id { get; set; }

        [DisplayName("نام اینگلیسی")]
        public string NameEn { get; set; }

        [DisplayName("نام فارسی")]
        public string NameFa { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ItemType, ItemTypeViewModel>();
        }
    }
}

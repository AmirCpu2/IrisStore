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
    public class PropertyDataGridViewModel : IHaveCustomMappings
    {
        [DisplayName("مشخصه")]
        public int Id { get; set; }

        [DisplayName("نام اینگلیسی")]
        public string NameEN { get; set; }

        [DisplayName("نام فارسی")]
        public string NameFA { get; set; }

        [DisplayName("ترتیب")]
        public int? SortingPriority { get; set; }

        [DisplayName("نمایش در اینترو محصول")]
        public bool ShowInIntro { get; set; }

        [DisplayName("گروه اصلی")]
        public string PropertyTypeFA { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Property, PropertyDataGridViewModel>()
                .ForMember(d => d.PropertyTypeFA, opt => opt.MapFrom(s => s.PropertyType.NameFA));
        }
    }
}

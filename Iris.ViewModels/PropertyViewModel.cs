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
    public class PropertyViewModel : IHaveCustomMappings
    {
        [DisplayName("مشخصه")]
        public int Id { get; set; }

        [DisplayName("نام اینگلیسی")]
        public string NameEN { get; set; }

        [DisplayName("نام فارسی")]
        public string NameFA { get; set; }

        [DisplayName("گروه اصلی")]
        public virtual PropertyType PropertyType { get; set; }

        [DisplayName("گروه اصلی")]
        public string PropertyTypeFA { get; set; }

        [DisplayName("مشخصه گروه اصلی")]
        public int PropertyTypeId { get; set; }
        
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Property, PropertyViewModel>()
                .ForMember(d => d.PropertyTypeFA, opt => opt.MapFrom(s => s.PropertyType.NameFA));

            configuration.CreateMap<Property, PropertyViewModel>().ReverseMap();
            configuration.CreateMap<PropertyViewModel, Property>().ReverseMap();
        }
    }
}

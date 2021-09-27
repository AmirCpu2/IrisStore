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
    public class PropertyTypeViewModel : IHaveCustomMappings
    {
        [DisplayName("مشخصه")]
        public int Id { get; set; }

        [DisplayName("نام اینگلیسی")]
        public string NameEN { get; set; }

        [DisplayName("نام فارسی")]
        public string NameFA { get; set; }

        [DisplayName("وضعیت بایگانی")]
        public bool IsArchive { get; set; }

        [DisplayName("وضعیت باز نویسی باز نویسی")]
        public bool IsEdited { get; set; }

        [DisplayName("تاریخ آخرین باز نویسی")]
        public DateTime? EditDate { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PropertyType, PropertyTypeViewModel>();
            configuration.CreateMap<PropertyTypeViewModel, PropertyType>();
        }
    }
}

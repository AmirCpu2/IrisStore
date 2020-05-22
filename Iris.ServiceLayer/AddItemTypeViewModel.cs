using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer
{
    public class AddItemTypeViewModel : IHaveCustomMappings
    {
        public int? Id { get; set; }
        public string NameEn { get; set; }
        public string NameFa { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AddItemTypeViewModel, ItemType>();

            configuration.CreateMap<ItemType, AddItemTypeViewModel>().ReverseMap();
        }
    }
}

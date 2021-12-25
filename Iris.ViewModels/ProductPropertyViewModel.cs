using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ViewModels
{
    public class ProductPropertyViewModel : IHaveCustomMappings
    {

        public int Id { get; set; } = 0;

        public int PropertyId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [MaxLength]
        public string Value { get; set; }

        public bool? ShowInIntro { get; set; } = false;

        [Required]
        public int DisplayOrder { get; set; } = 0;

        public PropertyViewModel Property { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductProperty, ProductPropertyViewModel>();
            configuration.CreateMap<ProductPropertyViewModel, ProductProperty>();

        }
    }
}

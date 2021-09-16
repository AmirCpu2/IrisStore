using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ViewModels
{
    public class UserFavoriteProductViewModel : IHaveCustomMappings
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime DateTime { get; set; }

        public virtual Product Product { get; set; }

        public virtual ApplicationUser User { get; set; }


        public virtual void CreateMappings(IConfiguration configuration)
        {
            //configuration.CreateMap<UserFavoriteProduct, UserFavoriteProductViewModel>()
            //    .ForMember(Favorite => Favorite., opt => opt.MapFrom(post => post.Category.Name));
        }

    }
}

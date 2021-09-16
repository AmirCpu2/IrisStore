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
    public class FavoriteProductMiniWidget : IHaveCustomMappings
    {

        public int Id { get; set; }

        public string Title { get; set; }
        public string SlugUrl { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }


        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, FavoriteProductMiniWidget>()
                .ForMember(Favorite => Favorite.Id, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Id))
                .ForMember(Favorite => Favorite.Title, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Title))
                .ForMember(Favorite => Favorite.Images, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Images))
                .ForMember(Favorite => Favorite.SlugUrl, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.SlugUrl))
                ;

            configuration.CreateMap<FavoriteProductMiniWidget, Product>()
                .ForMember(Favorite => Favorite.Id, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Id))
                .ForMember(Favorite => Favorite.Title, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Title))
                .ForMember(Favorite => Favorite.Images, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.Images))
                .ForMember(Favorite => Favorite.SlugUrl, opt => opt.MapFrom(FavoriteProduct => FavoriteProduct.SlugUrl))
                ;
        }


    }
}

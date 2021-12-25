using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Iris.DataLayer;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using PagedList;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Iris.ServiceLayer
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<ProductProperty> _ProductProperty;
        private readonly IDbSet<Property> _Property;

        public ProductPropertyService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _ProductProperty = unitOfWork.Set<ProductProperty>();
            _Property = unitOfWork.Set<Property>();
            _mappingEngine = mappingEngine;
        }

        public virtual async Task<bool> AddOrUpdateByString(string productProperty, int productId)
        {
            if(productProperty.Length < 1 || productId < 1)
                return false;

            var ProductPropertys = JsonConvert.DeserializeObject<ProductPropertyViewModel[]>(productProperty, settings: new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });

            foreach (var ProductProperty in ProductPropertys)
            {

                if (_ProductProperty.Where(q => q.ProductId == productId && q.PropertyId == ProductProperty.PropertyId).Any())
                {
                    var oldProductProperty = await _ProductProperty.FirstOrDefaultAsync(q => q.ProductId == productId && q.PropertyId == ProductProperty.PropertyId);

                    oldProductProperty.Value = ProductProperty.Value;

                    _ProductProperty.Attach(oldProductProperty);

                    _unitOfWork.Entry(oldProductProperty).State = EntityState.Modified;
                }
                else
                {
                    _ProductProperty.Add(new ProductProperty
                    {
                        ProductId = productId,
                        PropertyId = ProductProperty.PropertyId,
                        Value = ProductProperty.Value,
                        DisplayOrder = 0,
                        ShowInIntro = true
                    });
                }

            }
            
            if((ProductPropertys?.Length??0)>0)
                _unitOfWork.SaveAllChanges();

            return true;
        }

        public ProductPropertyViewModel GetProductProperty(int propertyTypeId, int productId)
        {
            var productProperty = new ProductPropertyViewModel();

            if (productId > 0)
            {
                productProperty = Mapper.Map<ProductPropertyViewModel>( _ProductProperty.FirstOrDefault(q => q.Property.PropertyTypeId == propertyTypeId && q.ProductId == productId) );
                
                
                

                if (productProperty != null)
                {
                    productProperty.Property.PropertyType = null;
                    return productProperty;
                }

            }


            var Property = _Property.FirstOrDefault(q => q.PropertyTypeId == propertyTypeId);

            Property.PropertyType.Properties = null;

            productProperty = new ProductPropertyViewModel()
            {
                DisplayOrder = 0,
                Id = 0,
                ProductId = productId,
                Property = Mapper.Map<PropertyViewModel>(Property)
            };

            return productProperty;



        }

        public List<ProductPropertyViewModel> GetProductPropertyList(int propertyTypeId, int productId)
        {
            var productPropertys = new List<ProductPropertyViewModel>();

            if (productId > 0)
            {

                var aproductPropertys = _ProductProperty.Where(q => q.Property.PropertyTypeId == propertyTypeId && q.ProductId == productId).ToList();

                productPropertys = aproductPropertys.Select( q => Mapper.Map<ProductProperty, ProductPropertyViewModel>(q)).ToList();


                if ( (productPropertys?.Count ?? 0) > 0 )
                {
                    for(int i=0; i < productPropertys.Count(); i++)
                    {
                        productPropertys[i].Property.PropertyType = new PropertyType();
                    }

                    return productPropertys;
                }
            }


            var Propertys = _Property.Where(q => q.PropertyTypeId == propertyTypeId).ToList();


            foreach (var Property in Propertys)
            {
                Property.PropertyType.Properties = null;
                productPropertys.Add(new ProductPropertyViewModel
                {
                    DisplayOrder = 0,
                    Id = 0,
                    ProductId = productId,
                    PropertyId = Property.Id
                    ,
                    Property = new PropertyViewModel
                    {
                        Id = Property.Id,
                        NameFA = Property.NameFA,
                        NameEN = Property.NameEN
                    },
                    Value = ""
                });
            }


            return productPropertys;
        }
    }
}

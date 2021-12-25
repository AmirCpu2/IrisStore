using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.DomainClasses;

namespace Iris.ServiceLayer.Contracts
{
    public interface IPropertyTypeService
    {
        void Add(PropertyType propertyType);
        void Delete(int id);
        void Edit(PropertyType propertyType);

        Task<IList<PropertyType>> GetListOfActualPropertyType(IList<string> propertyType);
        Task<IList<PropertyTypeViewModel>> GetAll();
        IList<PropertyTypeViewModel> GetAllNormal();
        Task<IList<PropertyTypeViewModel>> GetSearchProductsPropertyType();
        Task<IList<PropertyTypeViewModel>> SearchPropertyType(string term, int count);

        Task<DataGridViewModel<PropertyTypeDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
             NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        Task<PropertyTypeViewModel> GetById(int id);
        Task<IList<PropertyTypeViewModel>> AutoComplitPropertyType(int? productId, string searche);

    }
}

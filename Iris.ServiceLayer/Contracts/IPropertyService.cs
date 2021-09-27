using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IPropertyService
    {
        Task<DataGridViewModel<PropertyDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
              NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize, int propertyTypeId = 0);

        void Add(Property property);
        void Delete(int id);
        void Edit(Property property);
        Task<IList<PropertyViewModel>> GetAll();
        Task<IList<PropertyViewModel>> GetAllByPropertyTypeId(int propertyTypeId);
        Task<IList<PropertyViewModel>> GetAllByPropertyTypeName(string propertyTypeName);
        Task<string> GetPropertyName(int id);
        Task<IList<string>> GetPropertyNameList(List<int> id);
    }
}

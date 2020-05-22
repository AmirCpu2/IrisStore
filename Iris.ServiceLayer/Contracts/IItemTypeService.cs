using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IItemTypeService
    {
        void Add(ItemType itemType);
        Task<IList<ItemType>> GetListOfActualItemType(IList<string> itemType);
        Task<IList<ItemType>> GetAll();
        Task<IList<ItemTypeViewModel>> GetSearchProductsItemType();
        Task<IList<ItemTypeViewModel>> SearchItemType(string term, int count);

        Task<DataGridViewModel<ItemTypeDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
             NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void Delete(int id);
        void Edit(ItemType itemType);
    }
}

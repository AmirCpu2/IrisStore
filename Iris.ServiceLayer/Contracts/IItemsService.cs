using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IItemsService
    {
        Task<DataGridViewModel<ItemsDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
              NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize, int itemTypeId=0);

        void Add(Item item);
        void Delete(int id);
        void Edit(Item item);
        Task<IList<ItemsViewModel>> GetAll();
        Task<IList<ItemsViewModel>> GetAllByItemType(int itemTypeId);
        Task<string> GetItemName(int id);
        Task<IList<string>> GetItemNameList(List<int> id);
    }
}

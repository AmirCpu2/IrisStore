using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using PagedList;

namespace Iris.ServiceLayer
{
    public class ItemTypeService : IItemTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<ItemType> _itemType;

        public ItemTypeService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _itemType = unitOfWork.Set<ItemType>();
            _mappingEngine = mappingEngine;
        }

        public void Add(ItemType itemType)
        {
            _itemType.Add(itemType);
        }

        public async Task<DataGridViewModel<ItemTypeDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var query = _itemType.AsQueryable();

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<ItemTypeDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<ItemTypeDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public void Delete(int id)
        {
            var entity = new ItemType { Id = id };
            _itemType.Attach(entity);
            _itemType.Remove(entity);
        }

        public void Edit(ItemType ItemType)
        {
            _itemType.Attach(ItemType);

            _unitOfWork.Entry(ItemType).State = EntityState.Modified;
        }

        public Task<IList<ItemType>> GetListOfActualItemType(IList<string> itemType)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<ItemType>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<ItemTypeViewModel>> GetSearchProductsItemType()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<ItemTypeViewModel>> SearchItemType(string term, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}

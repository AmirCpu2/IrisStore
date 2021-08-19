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
    public class ItemsService : IItemsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Item> _Item;

        public ItemsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _Item = unitOfWork.Set<Item>();
            _mappingEngine = mappingEngine;
        }

        public void Add(Item Item)
        {
            _Item.Add(Item);
        }

        public async Task<DataGridViewModel<ItemsDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize, int itemTypeId = 0)
        {
            var query = _Item.AsQueryable().Where(q=>q.ItemTypeId.Equals(itemTypeId));

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<ItemsDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<ItemsDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public void Delete(int id)
        {
            var entity = new Item { Id = id };
            _Item.Attach(entity);
            _Item.Remove(entity);
        }

        public void Edit(Item Item)
        {
            _Item.Attach(Item);

            _unitOfWork.Entry(Item).State = EntityState.Modified;
        }

        public async Task<IList<ItemsViewModel>> GetAll()
        {
            return await _Item.AsQueryable().ProjectTo<ItemsViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<IList<ItemsViewModel>> GetAllByItemTypeById(int itemTypeId)
        {
            return await _Item.AsQueryable().Where(q=>q.ItemTypeId.Equals(itemTypeId))
                .ProjectTo<ItemsViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<IList<ItemsViewModel>> GetAllByItemTypeByName(string nameEn)
        {
            return await _Item.AsQueryable().Where(q => q.ItemType.NameEn.Equals(nameEn))
                .ProjectTo<ItemsViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<string> GetItemName(int id)
        {
            return await _Item.AsQueryable().Where(q => q.Id.Equals(id))
                .Select(q => q.NameFa)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<string>> GetItemNameList(List<int> id)
        {
            return await _Item.AsQueryable().Where(q => id.Contains(q.Id))
                .Select(q => q.NameFa).ToListAsync();
        }
    }
}

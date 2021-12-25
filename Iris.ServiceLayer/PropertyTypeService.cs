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
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<PropertyType> _PropertyType;

        public PropertyTypeService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _PropertyType = unitOfWork.Set<PropertyType>();
            _mappingEngine = mappingEngine;
        }

        public void Add(PropertyType propertyType)
        {
            _PropertyType.Add(propertyType);
        }

        public void Delete(int id)
        {
            var entity = new PropertyType { Id = id };
            _PropertyType.Attach(entity);
            _PropertyType.Remove(entity);
        }

        public void Edit(PropertyType propertyType)
        {
            _PropertyType.Attach(propertyType);

            _unitOfWork.Entry(propertyType).State = EntityState.Modified;
        }

        public async Task<IList<PropertyTypeViewModel>> GetAll()
        {
            return await _PropertyType.AsQueryable().ProjectTo<PropertyTypeViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<PropertyTypeViewModel> GetById(int id)
        {
            return Mapper.Map<PropertyTypeViewModel>(await _PropertyType.FirstOrDefaultAsync(q=>q.Id == id));
        }

        public async Task<DataGridViewModel<PropertyTypeDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form,
            DateTimeType dateTimeType, int page, int pageSize)
        {
            var query = _PropertyType.AsQueryable();

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<PropertyTypeDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<PropertyTypeDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public async Task<IList<PropertyType>> GetListOfActualPropertyType(IList<string> propertyType)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<PropertyTypeViewModel>> GetSearchProductsPropertyType()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<PropertyTypeViewModel>> SearchPropertyType(string term, int count)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<PropertyTypeViewModel>> AutoComplitPropertyType(int? productId, string searche)
        {
            var query = _PropertyType.AsQueryable();
            var PropertyTypes = query.Where(q => q.NameFA.Contains(searche));
           return await PropertyTypes.ProjectTo<PropertyTypeViewModel>(null, _mappingEngine).ToListAsync();

        }

        public IList<PropertyTypeViewModel> GetAllNormal()
        {
            return _PropertyType.AsQueryable().ProjectTo<PropertyTypeViewModel>(null, _mappingEngine)
                .ToList();
        }
    }
}

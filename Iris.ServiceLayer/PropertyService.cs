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
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Property> _Property;

        public PropertyService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _Property = unitOfWork.Set<Property>();
            _mappingEngine = mappingEngine;
        }

        public void Add(Property property)
        {
            _Property.Add(property);
        }

        public void Delete(int id)
        {
            var entity = new Property { Id = id };
            _Property.Attach(entity);
            _Property.Remove(entity);
        }

        public void Edit(Property property)
        {
            _Property.Attach(property);

            _unitOfWork.Entry(property).State = EntityState.Modified;
        }

        public async Task<IList<PropertyViewModel>> GetAll()
        {
            return await _Property.AsQueryable().ProjectTo<PropertyViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<IList<PropertyViewModel>> GetAllByPropertyTypeId(int propertyTypeId)
        {
            return await _Property.AsQueryable().Where(q => q.PropertyTypeId.Equals(propertyTypeId))
                .ProjectTo<PropertyViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<IList<PropertyViewModel>> GetAllByPropertyTypeName(string propertyTypeName)
        {
            return await _Property.AsQueryable().Where(q => q.PropertyType.NameEN.Equals(propertyTypeName))
                .ProjectTo<PropertyViewModel>(null, _mappingEngine)
                .ToListAsync();
        }

        public async Task<DataGridViewModel<PropertyDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form,
            DateTimeType dateTimeType, int page, int pageSize, int propertyTypeId = 0)
        {
            var query = _Property.AsQueryable().Where(q => q.PropertyTypeId.Equals(propertyTypeId));

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<PropertyDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<PropertyDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public async Task<string> GetPropertyName(int id)
        {
            return await _Property.AsQueryable().Where(q => q.Id.Equals(id))
                .Select(q => q.NameFA)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<string>> GetPropertyNameList(List<int> id)
        {
            return await _Property.AsQueryable().Where(q => id.Contains(q.Id))
                .Select(q => q.NameFA).ToListAsync();
        }
    }
}

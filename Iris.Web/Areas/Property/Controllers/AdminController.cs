using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer;
using Iris.ServiceLayer.Contracts;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Iris.ViewModels;

namespace Iris.Web.Areas.Property.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Property", AreaPrefix = "Property-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _mappingEngine = mappingEngine;
        }

        [Route("List")]
        public async virtual Task<ActionResult> Index(int id)
        {
            TempData["PropertyTypeId"] = id;

            var model = await _propertyTypeService.GetById(id);

            return View(model);
        }

        [Route("GetDataGridData")]
        [HttpPost]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request, int propertyTypeId)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _propertyService.GetDataGridSource(request.sidx + " " + request.sord,
                request, Request.Form, DateTimeType.Persian, pageIndex, pageSize, propertyTypeId);


            var totalRecords = list.TotalCount;
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);


            var jqGridData = new JqGridData
            {
                UserData = new // نمایش در فوتر
                {
                    Name = "جمع صفحه",
                    Price = 22
                },
                Total = totalPages,
                Page = request.page,
                Records = totalRecords,
                Rows = (list.Records.Select(property => new JqGridRowData
                {
                    Id = property.Id,
                    RowCells = new List<object>
                    {
                        property.Id.ToString(CultureInfo.InvariantCulture),
                        property.NameEN,
                        property.NameFA,
                        property.PropertyTypeFA,
                        property.ShowInIntro?"بله":"خیر",
                        property.SortingPriority,
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(PropertyViewModel propertyModel, int propertyTypeId)
        {
            var property = new DomainClasses.Property();

            propertyModel.PropertyTypeId = propertyTypeId;

            _mappingEngine.Map(propertyModel, property);

            if (propertyModel.Id > 0)
            {
                _propertyService.Edit(property);
            }
            else
            {
                _propertyService.Add(property);
            }

            await _unitOfWork.SaveAllChangesAsync();

            return Json(new { id = property.Id, success = true });
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _propertyService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }

        [Route("GetDestinationList")]

        public virtual ActionResult GetDestinationList(int? PropertyTypeId)
        {
            return View();
        }

    }
}
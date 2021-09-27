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


namespace Iris.Web.Areas.PropertyType.Controllers
{

    [Authorize(Roles = "Admin")]
    [RouteArea("PropertyType", AreaPrefix = "PropertyType-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IPropertyTypeService propertyTypeService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _propertyTypeService = propertyTypeService;
            _mappingEngine = mappingEngine;
        }

        [Route("List")]
        public virtual ActionResult Index()
        {
            //TempData["PropertyTypeId"] = id;
            return View();
        }

        [Route("GetDataGridData")]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _propertyTypeService.GetDataGridSource(request.sidx + " " + request.sord,
                request, Request.Form, DateTimeType.Persian, pageIndex, pageSize);


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
                        property.NameFA
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(PropertyTypeViewModel propertyTypeModel)
        {
            var propertyType = new DomainClasses.PropertyType();

            _mappingEngine.Map(propertyTypeModel, propertyType);

            if (propertyTypeModel.Id > 0)
            {
                propertyType.IsEdited = false;
                propertyType.EditDate = new DateTime();
                _propertyTypeService.Edit(propertyType);
            }
            else
            {
                propertyType.IsArchive = false;
                propertyType.IsEdited = false;
                //propertyType.EditDate = new DateTime() ;


                _propertyTypeService.Add(propertyType);
            }

            await _unitOfWork.SaveAllChangesAsync();

            return Json(new { id = propertyType.Id, success = true });
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _propertyTypeService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }

        

    }

}
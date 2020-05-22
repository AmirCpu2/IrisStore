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

namespace Iris.Web.Areas.ItemType.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("ItemType", AreaPrefix = "ItemType-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemTypeService _itemTypeService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IItemTypeService itemTypeService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _itemTypeService = itemTypeService;
            _mappingEngine = mappingEngine;
        }

        [Route("List")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [Route("GetDataGridData")]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _itemTypeService.GetDataGridSource(request.sidx + " " + request.sord,
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
                Rows = (list.Records.Select(ItemType => new JqGridRowData
                {
                    Id = ItemType.Id,
                    RowCells = new List<object>
                    {
                        ItemType.Id.ToString(CultureInfo.InvariantCulture),
                        ItemType.NameEn,
                        ItemType.NameFa,
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddItemTypeViewModel itemTypeModel)
        {
            var itemType = new DomainClasses.ItemType();
            
            _mappingEngine.Map(itemTypeModel, itemType);

            if (itemTypeModel.Id.HasValue)
            {
                _itemTypeService.Edit(itemType);
            }
            else
            {
                _itemTypeService.Add(itemType);
            }

            await _unitOfWork.SaveAllChangesAsync();

            return Json(new { id = itemType.Id, success = true });
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _itemTypeService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }

    }
}

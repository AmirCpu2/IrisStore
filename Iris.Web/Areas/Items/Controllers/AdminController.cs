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

namespace Iris.Web.Areas.Items.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Items", AreaPrefix = "Items-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemsService _itemsService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IItemsService itemsService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _itemsService = itemsService;
            _mappingEngine = mappingEngine;
        }

        [Route("List")]
        public virtual ActionResult Index(int id)
        {
            TempData["ItemTypeId"] = id;
            return View();
        }

        [Route("GetDataGridData")]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request,int itemTypeId)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _itemsService.GetDataGridSource(request.sidx + " " + request.sord,
                request, Request.Form, DateTimeType.Persian, pageIndex, pageSize, itemTypeId);


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
                Rows = (list.Records.Select(items => new JqGridRowData
                {
                    Id = items.Id,
                    RowCells = new List<object>
                    {
                        items.Id.ToString(CultureInfo.InvariantCulture),
                        items.NameEn,
                        items.NameFa,
                        items.ItemTypeFa,
                        items.DisplayPriority,
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddItemsViewModel itemsModel, int itemTypeId)
        {
            var items = new DomainClasses.Item();

            itemsModel.ItemTypeId = itemTypeId;

            _mappingEngine.Map(itemsModel, items);

            if (itemsModel.Id.HasValue)
            {
                _itemsService.Edit(items);
            }
            else
            {
                _itemsService.Add(items);
            }

            await _unitOfWork.SaveAllChangesAsync();

            return Json(new { id = items.Id, success = true });
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _itemsService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }

    }
}
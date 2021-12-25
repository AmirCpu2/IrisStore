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
namespace Iris.Web.Areas.ProductProperty.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("ProductProperty", AreaPrefix = "ProductProperty-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IProductPropertyService _productPropertyService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IPropertyService propertyService, IPropertyTypeService propertyTypeService, IProductPropertyService productPropertyService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _productPropertyService = productPropertyService;
            _mappingEngine = mappingEngine;
        }

        // GET: ProductProperty/Admin
        public ActionResult Index()
        {
            return View();
        }


        [Route("Add")]
        public virtual ActionResult Add(int? productId)
        {
            ViewData["PropertyTypeSelectList"] = new SelectList( _propertyTypeService
                                                                           .GetAllNormal(), "Id", "NameFa");

            ViewData["productId"] = productId;
            return View();
        }


        [Route("GetProductPropertyList")]
        public virtual ActionResult GetProductPropertyList(int propertyTypeId, int productId = 0)
        {

            if (propertyTypeId < 1)
                return null;
            var productPropertyList = _productPropertyService.GetProductPropertyList(propertyTypeId, productId);
            return Json(productPropertyList, JsonRequestBehavior.AllowGet); ;
           
        }


        [Route("AutoComplitProperty")]
        [HttpPost]
        public virtual async Task<JsonResult> AutoComplitPropertyType(int? productId, string searche)
        {

            return Json(await _propertyTypeService.AutoComplitPropertyType(productId, searche), JsonRequestBehavior.AllowGet); ;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Utilities;
using System.Security.Principal;
using System.Web.WebPages;


namespace Iris.Web.Areas.Lucene.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Manager", AreaPrefix = "Manager-Admin")]
    public class ManagerController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IItemsService _itemsService;
        private readonly IItemTypeService _itemTypeService;
        private readonly IProductService _productService;
        private readonly IApplicationUserManager _userManager;

        public ManagerController(IUnitOfWork unitOfWork, IApplicationUserManager userManager, IProductService productService, ICategoryService categoryService,
            IItemsService itemsService, IItemTypeService itemTypeService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _categoryService = categoryService;
            _itemsService = itemsService;
            _itemTypeService = itemTypeService;
            _mappingEngine = mappingEngine;
            _userManager = userManager;
        }

        // GET: Lucene/Manager
        public ActionResult Index()
        {
            return View();
        }

        [Route("Update")]
        public virtual async Task<ActionResult> Update() {

            var products = await _productService.GetAllForLuceneIndex();
            
            foreach(var product in products)
            {
                //Delete the product lucene.NET
                LuceneIndex.ClearLuceneIndexRecord(product.Id);

                //Index the new product lucene.NET
                LuceneIndex.AddUpdateLuceneIndex(new LuceneSearchModel
                {
                    ProductId = product.Id,
                    Description = product.Description,
                    Title = product.Title,
                    ProductStatus = product.ProductStatus.ToString(),
                    Price = product.Price.ToString(),
                    Discount = product.Discount.ToString(),
                    Image = product.Image,
                    SlugUrl = product.SlugUrl,
                    Category = "کالا‌ها"
                });
            }

            return Content("OK");
        }

    }
}
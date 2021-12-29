using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using PagedList;
using Utilities;

namespace Iris.Web.Areas.Product.Controllers
{
    [RouteArea("Product", AreaPrefix = "product")]
    public partial class SearchProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IItemsService _itemsService;
        private readonly IMappingEngine _mappingEngine;

        public SearchProductController(ICategoryService categoryService, IMappingEngine mappingEngine, IProductService productService, IItemsService itemsService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mappingEngine = mappingEngine;
            _itemsService = itemsService;
        }

        [Route("Search")]
        public virtual async Task<ActionResult> Index()
        {

            var model = new SerachProductIndexViewModel
            {
                Categories = new GroupsViewModel
                {
                    SelectedGroups = new List<GroupViewModel>(),
                    AvailableGroups = _mappingEngine.Map<IList<CategoryViewModel>, IList<GroupViewModel>>((await _categoryService.GetSearchProductsCategories())),
                },
                ProductColorSelectList = await _itemsService.GetAllByItemTypeByName(Enums.ItemType.ProductColor.ToString()),
                SellersSelectList = await _itemsService.GetAllByItemTypeByName(Enums.ItemType.Seller.ToString()),
                BrandSelectList = await _itemsService.GetAllByItemTypeByName(Enums.ItemType.Brand.ToString()),
                PricesMax = await _productService.GetAvailbleProductPriceMax(),
                PricesMin = await _productService.GetAvailbleProductPriceMin(),
                Discounts = await _productService.GetAvailableProductPrices()
            };

            return View(model);
        }

        [Route("GetProducts")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> GetProducts(SearchProductViewModel model)
        {
            var result = await _productService.SearchProduct(model);

            var productsAsIPagedList = new StaticPagedList<ProductWidgetViewModel>(result.Products, model.PageNumber, model.PageSize, result.TotalCount);

            return PartialView(MVC.Product.SearchProduct.Views._GetProducts,
                               productsAsIPagedList);
        }

        [Route("GetProductsJson")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<JsonResult> GetProductsJson(SearchProductViewModel model)
        {
            var result = await _productService.SearchProduct(model);

            var productsAsIPagedList = new StaticPagedList<ProductWidgetViewModel>(result.Products, model.PageNumber, model.PageSize, result.TotalCount);

            /*return PartialView(MVC.Product.SearchProduct.Views._GetProducts,
                               productsAsIPagedList);*/
            return Json(productsAsIPagedList, JsonRequestBehavior.AllowGet);
        }
    }
}
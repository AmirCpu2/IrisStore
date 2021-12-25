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

namespace Iris.Web.Areas.Product.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Product", AreaPrefix = "Product-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IItemsService _itemsService;
        private readonly IItemTypeService _itemTypeService;
        private readonly IProductService _productService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IProductPropertyService _productPropertyService;
        private readonly IApplicationUserManager _userManager;

        public AdminController(IUnitOfWork unitOfWork, IApplicationUserManager userManager, IProductService productService, ICategoryService categoryService,
            IItemsService itemsService, IItemTypeService itemTypeService, IPropertyService propertyService, IPropertyTypeService propertyTypeService,
            IProductPropertyService productPropertyService, IMappingEngine mappingEngine )
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _categoryService = categoryService;
            _itemsService = itemsService;
            _itemTypeService = itemTypeService;
            _mappingEngine = mappingEngine;
            _userManager = userManager;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _productPropertyService = productPropertyService;
        }

        [Route("List")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [Route("GetProducts")]
        public virtual async Task<ActionResult> GetProducts(JqGridRequest request, string hiddenColumns)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _productService.GetDataGridSource(request.sidx + " " + request.sord,
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
                Rows = (list.Records.Select(product => new JqGridRowData
                {
                    Id = product.Id,
                    RowCells = new List<object>
                    {
                        product.Id.ToString(CultureInfo.InvariantCulture),
                        product.Title,
                        product.ProductStatus,
                        product.Price,
                        product.Discount
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }

        [Route("Add")]
        public virtual async Task<ActionResult> AddProduct()
        {
            var productModel = new AddProductViewModel();

            //Set MuliSelectLists
            ViewData["CategoriesSelectList"] = new MultiSelectList(await _categoryService.GetAll(), "Name", "Name");

            ViewData["ProductColorSelectList"] = new MultiSelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.ProductColor.ToString()), "Id", "NameFa");
            
            ViewData["SellersSelectList"] = new SelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.Seller.ToString()), "Id", "NameFa");
            
            ViewData["BrandSelectList"] = new SelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.Brand.ToString()), "Id", "NameFa");

            return View(productModel);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> AddProduct(AddProductViewModel productModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriesSelectList"] = new MultiSelectList(
                    (await _categoryService.GetAll())
                    .Union(productModel.Categories.Select(x => new Category { Name = x }),
                    new ProductCategoryComparer()), "Name", "Name",
                    productModel.Categories);

                return View(productModel);
            }

            if (productModel.Count <= 0)
            {
                productModel.Count = 0;
                productModel.ProductStatus = ProductStatus.NotAvailable; ;
            }

            var addedProductImages = productModel.Images
                                    .Where(image => image.Id == null)
                                    .Select(image => new { image.Url, image.Name, image.ThumbnailUrl })
                                    .ToList();

            var deletedProductImages = new List<ProductImage>();

            var product = new DomainClasses.Product();

            foreach (var productImage in productModel.Images.Where(image => image.Id == null))
            {
                productImage.Url = Url.Content("~/UploadedFiles/ProductImages/" + Path.GetFileName(getFilePath(productImage.Name, Server.MapPath("~/UploadedFiles/ProductImages"))));
                productImage.ThumbnailUrl = Url.Content("~/UploadedFiles/ProductImages/Thumbs/" + Path.GetFileName(getFilePath(productImage.Name, Server.MapPath("~/UploadedFiles/ProductImages/Thumbs"))));
                productImage.DeleteUrl = "";
            }

            _mappingEngine.Map(productModel, product);
            
            if (productModel.Id.HasValue)
            {
                deletedProductImages = (await _productService.EditProduct(product)).ToList();
                TempData["message"] = "کالای مورد نظر با موفقیت ویرایش شد";
            }
            else
            {
                product.Title = productModel.Name;
                product.PostedDate = DateTime.Now;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                product.PostedByUserId = user.Id;
                await _productService.AddProduct(product);
                TempData["message"] = "کالای جدید با موفقیت ثبت شد";
            }

            await _unitOfWork.SaveAllChangesAsync();

            foreach (var productImage in addedProductImages)
            {
                var path = getFilePath(productImage.Name, Server.MapPath("~/UploadedFiles/ProductImages"));
                var thumbPath = getFilePath(productImage.Name, Server.MapPath("~/UploadedFiles/ProductImages/Thumbs"));

                System.IO.File.Move(Server.MapPath("~/Content/tmp/" + Path.GetFileName(productImage.Url)), path);
                System.IO.File.Move(Server.MapPath("~/Content/tmp/" + Path.GetFileName(productImage.ThumbnailUrl)), thumbPath);

            }

            foreach (var productImage in deletedProductImages)
            {
                var path = Server.MapPath("~/UploadedFiles/ProductImages/" + productImage.Name);
                var thumbPath = Server.MapPath("~/UploadedFiles/ProductImages/Thumbs/" + productImage.Name);
                System.IO.File.Delete(path);
                System.IO.File.Delete(thumbPath);
            }

            if (productModel.Id.HasValue)
            {
                LuceneIndex.ClearLuceneIndexRecord(productModel.Id.Value);
            }

            //Index the new product lucene.NET
            LuceneIndex.AddUpdateLuceneIndex(new LuceneSearchModel
            {
                ProductId = product.Id,
                Description = product.Body.RemoveHtmlTags(),
                Title = product.Title,
                ProductStatus = product.ProductStatus.ToString(),
                Price = product.Prices
                                .OrderByDescending(productPrice => productPrice.Date)
                                .Select(productPrice => productPrice.Price).
                                FirstOrDefault().
                                ToString(CultureInfo.InvariantCulture),
                Discount = product.Discounts
                                .OrderByDescending(productDiscount => productDiscount.StartDate)
                                .Select(productDiscount => productDiscount.Discount).
                                FirstOrDefault().
                                ToString(CultureInfo.InvariantCulture),
                Image = product.Images.OrderBy(image => image.Order)
                                        .Select(image => image.ThumbnailUrl)
                                        .FirstOrDefault(),
                SlugUrl = product.SlugUrl,
                Category = "کالا‌ها"
            });

            //Save ProductPeropertys
            await _productPropertyService.AddOrUpdateByString(productModel.ProductPeropertys, product.Id);


            //Save ProductItems
            //First Unjoin if id > 0
            /*if (productModel.Id.HasValue)
            {
                var productItems = await _productItemService.GetAllItemsByProductId(productModel?.Id ?? 0);
                if (productItems.Count > 0)
                    _productItemService.Unjoin(productItems.Select(q => q.ItemId).ToList(), productModel?.Id ?? 0);
            }

            //Last Join Items To product
            if (product.ProductItems.Count() > 0)
                _productItemService.Join(product.ProductItems.Select(q => q.ItemId).ToList(),
                                            product.Id);
                                                //product.ProductItems.Select(q => q.ItemTypeId).ToList());*/

            return RedirectToAction(MVC.Product.Admin.ActionNames.Index);
        }

        [Route("Edit/{id:int?}")]
        public virtual async Task<ActionResult> EditProduct(int id)
        {
            var selectedProduct = await _productService.GetProductForEdit(id);

            ViewData["CategoriesSelectList"] =
                new MultiSelectList(await _categoryService.GetAll(), "Name", "Name", selectedProduct.Categories);

            ViewData["ProductColorSelectList"] = new MultiSelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.ProductColor.ToString()), "Id", "NameFa", selectedProduct.ProductColor);

            ViewData["SellersSelectList"] = new SelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.Seller.ToString()), "Id", "NameFa", selectedProduct.Sellers);

            ViewData["BrandSelectList"] = new SelectList(await _itemsService
                                                                           .GetAllByItemTypeByName(Enums.ItemType.Brand.ToString()), "Id", "NameFa", selectedProduct.Brand);

            return View(MVC.Product.Admin.Views.AddProduct, selectedProduct);
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);

            var deletedImages = await _productService.GetProductImages(id);

            await _unitOfWork.SaveAllChangesAsync();

            LuceneIndex.ClearLuceneIndexRecord(id);

            foreach (var productImage in deletedImages)
            {
                var path = Server.MapPath("~/UploadedFiles/ProductImages/" + productImage);
                var thumbPath = Server.MapPath("~/UploadedFiles/ProductImages/Thumbs/" + productImage);
                System.IO.File.Delete(path);
                System.IO.File.Delete(thumbPath);
            }

            return Json(true);
        }


        private string getFilePath(string fileName, string path)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFullPath = Path.Combine(path, fileName); ;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = $"{fileNameOnly}({count++})";
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }

    }




    class ProductCategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Category obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
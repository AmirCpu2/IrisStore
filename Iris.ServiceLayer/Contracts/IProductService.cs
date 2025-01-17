﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IProductService
    {
        Task AddProduct(Product product);

        Task<IList<ProductImage>> EditProduct(Product editedProduct);

        Task<AddProductViewModel> GetProductForEdit(int productId);

        Task<DataGridViewModel<ProductDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
            NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void DeleteProduct(int productId);
        Task<IList<ProductWidgetViewModel>> GetNewestProducts(int count);
        List<ProductWidgetViewModel> GetNewestProductsForce(int count);
        Task<IList<ProductWidgetViewModel>> GetMostViewedProducts(int count);
        Task<IList<ProductWidgetViewModel>> GetSuggestionProducts(int count);
        List<ProductWidgetViewModel> GetSuggestionProductsForce(int count);
        Task<IList<ProductWidgetViewModel>> GetPopularProducts(int count);
        Task<IList<decimal>> GetAvailableProductPrices();
        Task<decimal> GetAvailbleProductPriceMax();
        Task<decimal> GetAvailbleProductPriceMin();
        Task<IList<decimal>> GetAvailableProductDiscounts();
        Task<ProductSearchPagedList> SearchProduct(SearchProductViewModel searchModel);
        Task<ProductPageViewModel> GetProductPage(int productId);
        ProductPageViewModel GetProductPageNormal(int productId);
        Task UpdateViewNumber(int productId);
        Task SaveRating(int productId, double rating);
        Task<IList<LueneProduct>> GetAllForLuceneIndex();
        Task<IList<string>> GetProductImages(int productId);
        Task<IList<ProductOrderViewModel>> GetProductsOrders(int[] productIds);
        Task<ProductOrderViewModel> GetProductOrder(int productId);
    }

    public class LueneProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string SlugUrl { get; set; }
        public string Category { get; set; }
    }

    public class ProductSearchPagedList
    {
        public IList<ProductWidgetViewModel> Products { get; set; }
        public int TotalCount { get; set; }
    }

}


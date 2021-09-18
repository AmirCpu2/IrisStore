using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Iris.DataLayer;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.Web.Helpers;

namespace Iris.Web.Areas.Product.Controllers
{
    [RouteArea("Product", AreaPrefix = "product")]
    public partial class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;
        private readonly IApplicationUserManager _userManager;

        public HomeController(IUnitOfWork unitOfWork, IProductService productService, IFavoriteService favoriteService, IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        [Route("{id:int?}/{slugUrl?}")]
        public virtual async Task<ActionResult> Index(int id)
        {
            var model = await _productService.GetProductPage(id);

            ViewData["SimilarProducts"] = LuceneIndex.GetMoreLikeThisProjectItems(id)
                    .Where(item => item.Category == "کالا‌ها").Skip(1).Take(8).ToList();

            await _unitOfWork.SaveAllChangesAsync(false);

            var keywords = "";

            if (model.Categories != null && model.Categories.Any())
            {
                keywords = model.Categories.Aggregate(keywords, (current, category) => current + (category.Name + "-"));
            }

            keywords = keywords.Substring(0, keywords.Length - 1);

            ViewBag.Keywords = keywords;

            ViewBag.MetaDescription = model.MetaDescription;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.IsFavorite = await _favoriteService.GetFavoriteProductState(user?.Id??0, id);

            return View(model);
        }

        [Route("RecommendedProductCarousel")]
        public virtual ActionResult RecommendedProductCarousel()
        {
            var recommendedProducts = (_productService.GetSuggestionProductsForce(9));

            return View(MVC.Widgets.Views.ContainerSliderWidget, new Iris.Web.ViewModels.ProductSliderWidgetViewModel
            {
                Title = "محصولات پیشنهادی برای شما",
                CarouselId = "RecommendedProductCarousel",
                Products = recommendedProducts,
                LinkText = "مشاهده همه",
                Link = $"{Url.Action(MVC.Product.SearchProduct.ActionNames.Index, MVC.Product.SearchProduct.Name, new { area = MVC.Product.Name })}#/page/all/empty/1/ViewNumber/desc",
                ShowCount = 20
            });
        }

        [Route("SaveRatings")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> SaveRatings(int id, double value)
        {
            var sectionType = "Product";

            if (!this.HttpContext.CanUserVoteBasedOnCookies(id, sectionType))
                return Content("nok"); //اعلام فقط یکبار مجاز هستید رای دهید

            await _productService.SaveRating(id, value);

            await _unitOfWork.SaveAllChangesAsync(false);

            return Content("ok"); //اعلام موفقیت آمیز بودن ثبت اطلاعات
        }




    }
}
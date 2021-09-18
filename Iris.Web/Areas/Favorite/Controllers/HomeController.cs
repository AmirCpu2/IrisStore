using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Iris.DataLayer;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.Web.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Iris.Web.Areas.Favorite.Controllers
{
    [RouteArea("Favorite", AreaPrefix = "Favorite")]
    public class HomeController : Controller
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

        // GET: Favorite/MyProdcts
        [Route("MyProdcts")]
        public virtual async Task<ActionResult> MyProdcts()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var FP = user.UserFavoriteProducts.Select(q=> Mapper.Map<Iris.ViewModels.UserFavoriteProductViewModel>(q)).ToList();

            return View(FP);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<bool> AddProductFavorite(int id)
        {
            if (id < 1)
                return false;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            await _favoriteService.AddFavoriteProduct(user.Id, id);

            await _unitOfWork.SaveAllChangesAsync(false);

            return true;
        }

        [Route("Remove")]
        [HttpPost]
        public virtual async Task<bool> RemoveProductFavorite(int id)
        {
            if (id < 1)
                return false;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            await _favoriteService.RemoveFavoriteProduct(user.Id, id);

            await _unitOfWork.SaveAllChangesAsync(false);

            return true;
        }



        //public virtual async Task<ActionResult> 


    }
}
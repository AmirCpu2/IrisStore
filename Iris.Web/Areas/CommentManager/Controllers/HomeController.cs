using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;

namespace Iris.Web.Areas.CommentManager.Controllers
{
    [RouteArea("CommentManager", AreaPrefix = "Comment")]
    public partial class HomeController : Controller
    {
        private IApplicationUserManager _userManager;
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        private readonly IFactorService _factorService;

        public HomeController(IApplicationUserManager userManager, IProductService productService, ICommentService commentService, IFactorService factorService)
        {
            _userManager = userManager;
            _productService = productService;
            _commentService = commentService;
            _factorService = factorService;

        }

        // GET: CommentManager/Home
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("AddOrUpdate")]
        public virtual async Task<ActionResult> AddOrUpdate(int productId)
        {
            var product = await _productService.GetProductPage(productId);

            var userId = _userManager.GetCurrentUserId();
            
            if (product == null || userId < 1)
                return new HttpNotFoundResult();

            var comment = await _commentService.GetAllCommentByProductIdAndUser(productId, userId) ?? new CommentViewModel();

            comment.ProductVM = product;

            return View(comment);
        }
        
        [Route("AddOrUpdate")]
        [HttpPost]
        public virtual async Task<ActionResult> AddOrUpdate(CommentViewModel entity)
        {
            //if (!ModelState.IsValid)
            //{
                entity.UserId = _userManager.GetCurrentUserId();
                await _commentService.AddOrUpdate(entity);
            //}

            return RedirectToAction("Index","Home",new { @area = "product" , @id = entity.ProductVM.Id, @slugUrl = entity.ProductVM.SlugUrl });
        }

        //[ChildActionOnly]
        [Route("ShowCommentsProduct")]
        public virtual ActionResult ShowCommentsProduct(int productId)
        {
            ViewData["ProductId"] = productId;

            var currentUserId = _userManager.GetCurrentUserId();

            var comments = _commentService.GetAllCommentByProductIdByState(productId, (int)Utilities.Enums.CommentState.Confirmed);

            var product = _productService.GetProductPageNormal(productId);

            var showCommentsProduct = new List<ShowCommentsProductViewModel>();

            foreach(var comment in comments)
            {
                showCommentsProduct.Add(new ShowCommentsProductViewModel() { 
                    Comment = AutoMapper.Mapper.Map<CommentViewModel>(comment),
                    ColorNameFa = product.ProductColors.FirstOrDefault().NameFa,
                    ProductNameFa = product.Name,
                    ColorCode = Utilities.Color.GetColorCode(product.ProductColors.FirstOrDefault().NameEn),
                    IsBuyer = _factorService.UserBoughtProduct(productId, comment?.UserId??0), 
                    SellerFa = product.Sellers.NameFa,
                    UserFullName = _userManager.GetFullNameById(comment?.UserId??0)
                });
            }

            ViewData["CurrentUserIsCommenting"] = comments.Where(q=> q.UserId == currentUserId).Any();
            

            return View("_showCommentsProduct", showCommentsProduct);
        }

    }
}
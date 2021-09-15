using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Iris.DataLayer;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Microsoft.AspNet.Identity;
using Iris.Web.ViewModels.Identity;

namespace Iris.Web.Areas.ShoppingCart.Controllers
{
    [RouteArea("ShoppingCart", AreaPrefix = "ShpppingCart")]
    public partial class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserManager _userManager;


        public HomeController(IProductService productService, IApplicationUserManager userManager, IShoppingCartService shoppingCartService,
            IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Route]
        public virtual ActionResult Index()
        {
            return View();
        }

        [Route("GetOrdersList")]
        public virtual async Task<ActionResult> GetOrdersList(int[] productIds)
        {
            return PartialView(await _productService.GetProductsOrders(productIds));
        }

        [Route("GetProductList")]
        public virtual async Task<JsonResult> GetProductList(int[] productIds)
        {
            return Json(await _productService.GetProductsOrders(productIds), JsonRequestBehavior.AllowGet);
        }

        [Route("GetCartEmpty")]
        public virtual ActionResult GetCartEmpty()
        {
            ViewData["NewProducts"] = ( _productService.GetNewestProductsForce(8));

            ViewData["RecommendedProducts"] = ( _productService.GetSuggestionProductsForce(9));

            return PartialView();
        }

        [Route("CreateFactor")]
        [HttpGet]
        [Authorize]
        public virtual async Task<ActionResult> CreateFactor()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Mobile = user.Mobile;
                ViewBag.Address = user.Address;
            }
            return View();
        }

        [Route("CreateFactor")]
        [HttpPost]
        public virtual async Task<ActionResult> CreateFactor(CreateFactorViewModel factorViewModel)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.Mobile) && string.IsNullOrEmpty(user.Address))
                {
                    user.FirstName = factorViewModel.Name;
                    user.LastName = factorViewModel.LastName;
                    user.Mobile = factorViewModel.PhoneNumber;
                    user.Address = factorViewModel.Address;
                }

                await _shoppingCartService.CreateFactor(factorViewModel);
                await _unitOfWork.SaveAllChangesAsync();
                return Content(Url.Action("UserFactor", "Home", new { area = "ShoppingCart" }));
            }
        }


        [Route("UserFactor")]
        [HttpGet]
        public virtual async Task<ActionResult> UserFactor()
        {
            return View(await _shoppingCartService.GetUserFactor(Convert.ToInt32(User.Identity.GetUserId())));
        }

        [Route("Peymen")]
        [HttpPost]
        public virtual async Task<string> Peymen(CreateFactorViewModel factorViewModel)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            int factorId;
            if (user == null)
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.Mobile) && string.IsNullOrEmpty(user.Address))
                {
                    user.FirstName = factorViewModel.Name;
                    user.LastName = factorViewModel.LastName;
                    user.Mobile = factorViewModel.PhoneNumber;
                    user.Address = factorViewModel.Address;
                }

                factorId = await _shoppingCartService.CreateFactor(factorViewModel);
            }

            //Peyment cal

            var factorTotalPrice = (int)0;

            foreach (var factorProduct in factorViewModel.Products)
            {
                var selectedProduct = await _productService.GetProductOrder(factorProduct.ProductId);

                if (selectedProduct == null)
                    return null;

                factorTotalPrice += ((int)selectedProduct.CurrentPrice * factorProduct.Count);
            }


            System.Net.ServicePointManager.Expect100Continue = false;
            Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;

            int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", factorTotalPrice, "تست درگاه زرین پال در راه ابریشم", "Amircpu2@gmail.com", "09217159257", $"http://localhost/shpppingcart/Verify?id="+ factorId, out Authority);

            if (Status == 100)
            {
                //Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                ViewBag.Error = "Error : " + Status;
            }
            return "https://sandbox.zarinpal.com/pg/StartPay/" + Authority;

        }

        [Route("Verify")]
        public virtual async Task<ActionResult> Verify(int id)
        {
            var order = await _shoppingCartService.GetForEdit(id);
            order.Status = Iris.DomainClasses.FactorStatus.Cancelled;

            //Peyment cal

            var factorTotalPrice = 0;

            foreach (var factorProduct in order.Products)
            {
                var selectedProduct = await _productService.GetProductOrder(factorProduct.ProductId);

                if (selectedProduct == null)
                    return null;

                factorTotalPrice += ((int)selectedProduct.CurrentPrice * factorProduct.Count);
            }

            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {
                    long RefID;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();

                    int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), factorTotalPrice, out RefID);

                    if (Status == 100)
                    {
                        ViewBag.IsSuccess = true;
                        ViewBag.RefId = RefID;
                        order.Status = Iris.DomainClasses.FactorStatus.Paid;
                    }
                    else
                    {
                        ViewBag.Status = Status;
                    }
                    ViewBag.steps = 3;
                }
                else
                {
                    Response.Write("Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString());
                }
            }
            else
            {
                Response.Write("Invalid Input");
            }


            //order
            await _shoppingCartService.Edit(order);

            await _unitOfWork.SaveAllChangesAsync();

            return View();
        }

    }
}
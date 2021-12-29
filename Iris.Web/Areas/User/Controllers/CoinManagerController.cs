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
using Iris.ServiceLayer;
using Microsoft.Owin.Security;
using AutoMapper;
using AutoMapperContracts;
using Utilities;
using Iris.DomainClasses;

namespace Iris.Web.Areas.User.Controllers
{
    [RouteArea("user", AreaPrefix = "CoinManager")]
    public partial class CoinManagerController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ICoinFactorService _coinFactorService;


        public CoinManagerController(
            ICoinFactorService coinFactorService,
            IUnitOfWork unitOfWork,
            IApplicationUserManager userManager,
            IApplicationSignInManager applicationSignInManager,
            IAuthenticationManager authenticationManager)
        {
            _coinFactorService = coinFactorService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [Route("Index")]
        [HttpGet]
        public virtual async Task<ActionResult> Index()
        {
            var currentUserId = _userManager.GetCurrentUserId();

            if(currentUserId < 1)
            {
                return HttpNotFound();
            }

            var coinFactorList = await _coinFactorService.GetAllByUserId(currentUserId);

            return View(coinFactorList);
        }

        [Route("BuyCoins")]
        [HttpGet]
        public virtual ActionResult BuyCoins(bool? isSuccess)
        {
            ViewBag.IsSuccess = isSuccess;

            return View();
        }

        [Route("CreateCoinFactor")]
        [HttpGet]
        public virtual ActionResult CreateCoinFactor(int coinPacketId)
        {
            if (coinPacketId < 1)
                return new HttpNotFoundResult();

            var currentUserId = _userManager.GetCurrentUserId();

            if (currentUserId < 1)
                return new HttpNotFoundResult();

            var coinFactor = new CoinFactor
            {
                BuyDate = DateTime.Now,
                CoinPacketId = coinPacketId,
                StatusId = (int)Enums.CoinFactorStatus.Paying,
                UserId = currentUserId,
            };

            var coinFactorVM = _coinFactorService.Add(coinFactor);

            //Peyment cal

            var factorTotalPrice = int.Parse(((Enums.CoinPacketPrice)coinFactorVM.CoinPacketId).GetEnumDescription()) * 1000;


            System.Net.ServicePointManager.Expect100Continue = false;
            Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();
            
            string Authority;

            int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", factorTotalPrice, "تست درگاه زرین پال در راه ابریشم", "Amircpu2@gmail.com", "09217159257", $"http://localhost/CoinManager/Verify?id=" + coinFactorVM.Id, out Authority);

            if (Status == 100)
            {
                Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                ViewBag.Error = "Error : " + Status;
            }
            
            return RedirectPermanent("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);

        }

        [Route("Verify")]
        public virtual async Task<ActionResult> Verify(Guid id)
        {
            var coinFactorVM = await _coinFactorService.GetOneById(id);

            if (coinFactorVM == null)
                return new HttpNotFoundResult();

            coinFactorVM.StatusId = (int) Enums.CoinFactorStatus.Cancelled;

            //Peyment cal
            var factorTotalPrice = int.Parse(((Enums.CoinPacketPrice)coinFactorVM.CoinPacketId).GetEnumDescription()) * 1000;

            long RefID;
            System.Net.ServicePointManager.Expect100Continue = false;
            Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();

            int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), factorTotalPrice, out RefID);

            if (Status == 100)
            {
                ViewBag.IsSuccess = true;
                ViewBag.RefId = RefID;
                coinFactorVM.RefId = RefID.ToString();
                coinFactorVM.StatusId = (int)Enums.CoinFactorStatus.Paid;

                var PackPrice = coinFactorVM.CoinPacket.GetEnumDescription();

                await _userManager.AddCoin(int.Parse(PackPrice), int.Parse(User.Identity.GetUserId()) );
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Status = Status;
                coinFactorVM.StatusId = (int)Enums.CoinFactorStatus.Cancelled;
            }
            //Update
            _coinFactorService.Edit(Mapper.Map<CoinFactorViewModel, CoinFactor>(coinFactorVM));

            return RedirectToAction("BuyCoins", new { @isSuccess =ViewBag.IsSuccess });
        }

    }
}
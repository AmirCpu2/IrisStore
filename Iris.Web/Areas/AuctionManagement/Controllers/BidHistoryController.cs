using Iris.DataLayer;
using Iris.ServiceLayer.Contracts;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Iris.Web.Areas.AuctionManagement.Controllers
{
    [RouteArea("AuctionManagement", AreaPrefix = "BidHistory")]
    public class BidHistoryController : Controller
    {

        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IBidHistoryService _bidHistoryService;
        private readonly IAuctionItemService _auctionItemService;


        public BidHistoryController(
            IAuctionItemService auctionItemService,
            IUnitOfWork unitOfWork,
            IApplicationUserManager userManager,
            IApplicationSignInManager applicationSignInManager,
            IAuthenticationManager authenticationManager,
            IBidHistoryService bidHistoryService)
        {
            _auctionItemService = auctionItemService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
            _bidHistoryService = bidHistoryService;
        }

        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("AddBid")]
        [HttpGet]
        public async Task<ActionResult> AddBid(int auctionItemId)
        {

            var curentUserId = _userManager.GetCurrentUserId();
            var curentUser = await _userManager.GetCurrentUserAsync();

            if(curentUserId < 1)
            {
                return RedirectToAction("Index", "Auction", new { @area = "AuctionManagement", @id = auctionItemId, @state = (int)Enums.BidRequestState.NotAutorize });
            }

            var auctionItem = await _auctionItemService.GetOneById(auctionItemId);

            if (auctionItem == null)
            {
                return HttpNotFound();
            }

            decimal price = 0;
            if(auctionItem?.BidHistories.Count > 0)
            {
                price += auctionItem?.BidHistories?.OrderByDescending(q=>q.BidPrice).FirstOrDefault().BidPrice ?? 0;

            }
            else
            {
                price = auctionItem.MiniPrice;
            }
            
            int disCoin = ((int)auctionItem.MiniPrice) / 1000;

            if(disCoin > (curentUser?.BidCount??0))
            {
                return RedirectToAction("Index", "Auction", new { @area = "AuctionManagement", @id = auctionItemId, @state = (int)Enums.BidRequestState.NotMatchCoin });

            }

            //Dis User Coin
            await _userManager.ReduceCoin(disCoin, curentUserId);

            //Add bid
            var bidHistoryResult = _bidHistoryService.Add(new DomainClasses.BidHistory
            {
                AuctionItemId = auctionItemId,
                BidDate = DateTime.Now,
                BidPrice = price + (decimal)((int)auctionItem.MiniPrice * 0.10),
                UserId = curentUserId
            });

            return RedirectToAction("Index", "Auction", new { @area = "AuctionManagement", @id = auctionItemId, @state = (int)Enums.BidRequestState.Complited });
        }
    }
}
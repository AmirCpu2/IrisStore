using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Areas.AuctionManagement.Controllers
{
    [RouteArea("AuctionManagement", AreaPrefix = "SearchAuction")]
    public class SearchAuctionController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IAuctionItemService _auctionItemService;


        public SearchAuctionController(
            IAuctionItemService auctionItemService,
            IUnitOfWork unitOfWork,
            IApplicationUserManager userManager,
            IApplicationSignInManager applicationSignInManager,
            IAuthenticationManager authenticationManager)
        {
            _auctionItemService = auctionItemService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [Route("Index")]
        public virtual ActionResult Index(int page = 1, int sortState = 0)
        {

            var take = 10;

            var skip = page * take - take;
            //q.StopDate > DateTime.Now &&
            var auctionItemQuery = _auctionItemService.GetAll_asQuery();

            var lastPageNumber = auctionItemQuery.Count() / take;

            IQueryable<AuctionItem> auctionItemListOrder = auctionItemQuery.OrderBy(q => q.Id);

            switch ((Utilities.Enums.SortState)sortState)
            {
                case Utilities.Enums.SortState.ASNews:
                    auctionItemListOrder = auctionItemQuery.OrderBy(q => q.Id);
                    break;
                case Utilities.Enums.SortState.DSNews:
                    auctionItemListOrder = auctionItemQuery.OrderByDescending(q => q.Id);
                    break;
                case Utilities.Enums.SortState.ASStartMony:
                    auctionItemListOrder = auctionItemQuery.OrderBy(q => q.MiniPrice);
                    break;
                case Utilities.Enums.SortState.DSStartMony:
                    auctionItemListOrder = auctionItemQuery.OrderByDescending(q => q.MiniPrice);
                    break;
                case Utilities.Enums.SortState.ASDateTime:
                    auctionItemListOrder = auctionItemQuery.OrderBy(q => q.StartDate);
                    break;
                case Utilities.Enums.SortState.DSDateTime:
                    auctionItemListOrder = auctionItemQuery.OrderByDescending(q => q.StartDate);
                    break;
            }

            var auctionItemList = auctionItemListOrder
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(Mapper.Map<AuctionItem, AuctionItemViewModel>)
                .ToList();
            ;

            var SearchAuctionViewModel = new SearchAuctionViewModel() {
                AuctionItemList = auctionItemList,
                AuctionAvailableItemCount = auctionItemList.Count(),
                LastPageNumber = lastPageNumber,
                PageNumber = page,
                SortState = (Utilities.Enums.SortState)sortState
            };

            ViewData["CurrentUserId"] = _userManager?.GetCurrentUserId()??0;

            return View(SearchAuctionViewModel);
        }



    }
}
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Areas.AuctionManagement.Controllers
{
    [RouteArea("AuctionManagement", AreaPrefix = "Auction")]
    public class AuctionController : Controller
    {

        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IAuctionItemService _auctionItemService;


        public AuctionController(
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
        [HttpGet]
        public async Task<ActionResult> Index(int id, int? state)
        {
            if (id < 1) 
                return HttpNotFound();

            ViewBag.IsSuccess = state;

            var auctionItem = await _auctionItemService.GetOneById(id);
            
            return View(auctionItem);
        }


        [Route("AddOrUpdate")]
        [HttpGet]
        public async Task<ActionResult> AddOrUpdate(int? id)
        {
            var auctionItem = new AuctionItemViewModel();

            if (id > 0)
            {
                auctionItem = await _auctionItemService.GetOneById(id??0);
            }


            return View(auctionItem);
        }

        [Route("AddOrUpdate")]
        [HttpPost]
        public ActionResult AddOrUpdate(AuctionItemViewModel entity)
        {

            if (entity.ImageFileUpload != null)
            {
                //Upload Image
                var allowedExtensions = new[] {
                    ".Jpg", ".png", ".jpg", "jpeg"
                };

                var fileName = Path.GetFileName(entity.ImageFileUpload.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(entity.ImageFileUpload.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + DateTime.Now.ToString("yyyyy-MM-dd-HH-MM-ss") + ext; //appending the name with id  
                                                                                                      // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/AuctionImages"), myfile);
                    entity.ImageAddress = $"/UploadedFiles/AuctionImages/{myfile}";
                    entity.ImageName = fileName;
                    entity.ImageFileUpload.SaveAs(path);
                }
                else
                {
                    ViewBag.FileUploadMessage = "بارگذاری تصویر ناموفق بود";
                    return View(entity);
                }
            }

            entity.StartDate =  Utilities.PersianDateUtils.ConvertPersianToGregorianDate(entity.StartDateView) ?? entity.StartDate;
            entity.StopDate =  Utilities.PersianDateUtils.ConvertPersianToGregorianDate(entity.StopDateView) ?? entity.StopDate;


            if (entity.Id > 0)
            {

                entity.EditDate = DateTime.Now;
                entity.IsEdit = true;

                var a = AutoMapper.Mapper.Map<AuctionItemViewModel, AuctionItem>(entity);
                _auctionItemService.Edit(a);

            }
            else
            {
                entity.PostedByUserId = _userManager.GetCurrentUserId();

                //save AuctionItem
                var auctionItemSaved = _auctionItemService.Add(AutoMapper.Mapper.Map<AuctionItemViewModel, AuctionItem>(entity));

            }


            return RedirectToAction("Index", "Home", new { @area = "User" });
        }

        [Route("UserAuctionList")]
        [HttpGet]
        public async Task<ActionResult> UserAuctionList()
        {
            var currentUserId = _userManager.GetCurrentUserId();

            if(currentUserId < 1)
            {
                return HttpNotFound();
            }    

            var auctionItemList = await _auctionItemService.GetAllByUserId(currentUserId);


            return View(auctionItemList);
        }
        
        [Route("UserAuctionListWinner")]
        [HttpGet]
        public async Task<ActionResult> UserAuctionListWinner()
        {
            var currentUserId = _userManager.GetCurrentUserId();

            if(currentUserId < 1)
            {
                return HttpNotFound();
            }    

            var auctionItemList = await _auctionItemService.GetAllWinnByUserId(currentUserId);


            return View(auctionItemList);
        }

    }
}
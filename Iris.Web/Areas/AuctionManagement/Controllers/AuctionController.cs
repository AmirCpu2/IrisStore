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
        public ActionResult AddOrUpdate(AuctionItemViewModel entity, HttpPostedFileBase file)
        {

            //Upload Image
            var allowedExtensions = new[] {
                ".Jpg", ".png", ".jpg", "jpeg"
            };

            var fileName = Path.GetFileName(entity.ImageFileUpload.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(entity.ImageFileUpload.FileName); //getting the extension(ex-.jpg)  
            if(allowedExtensions.Contains(ext)) //check what type of extension  
            {
                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                string myfile = name + "_" + DateTime.Now.ToString("yyyyy-MM-dd-HH-MM-ss") + ext; //appending the name with id  
                // store the file inside ~/project folder(Img)  
                var path = Path.Combine(Server.MapPath("~/UploadedFiles/AuctionImages"), myfile);
                entity.ImageAddress = path;
                entity.ImageName = fileName;
                entity.ImageFileUpload.SaveAs(path);
            }
            else
            {
                ViewBag.FileUploadMessage = "بارگذاری تصویر ناموفق بود";
                return View(entity);
            }

            if(entity.Id > 0)
            {
                entity.EditDate = DateTime.Now;

                //Edit AuctionItem
                _auctionItemService.Edit(AutoMapper.Mapper.Map<AuctionItemViewModel, AuctionItem>(entity));

            }
            else
            {
                entity.PostedByUserId = _userManager.GetCurrentUserId();

                //save AuctionItem
                var auctionItemSaved = _auctionItemService.Add(AutoMapper.Mapper.Map<AuctionItemViewModel, AuctionItem>(entity));

            }


            return RedirectToAction("Index", "Home", new { @area = "User" });
        }



    }
}
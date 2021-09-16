﻿using System;
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

namespace Iris.Web.Areas.User.Controllers
{
    [RouteArea("User", AreaPrefix = "User")]
    public partial class HomeController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IProductService _productService;


        public HomeController(IProductService productService, IUnitOfWork unitOfWork, IApplicationUserManager userManager, IApplicationSignInManager applicationSignInManager,
            IAuthenticationManager authenticationManager)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [Route]
        public virtual ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.UserProfileSuccessfully ? "اطلاعات شما با موفقیت بروز رسانی شد!"
             : message == ManageMessageId.ChangePasswordSuccess ? "گذروازه شما با موفقیت تغییر یافت."
             : message == ManageMessageId.SetPasswordSuccess ? "گذروازه شما با موفقیت بازنشانی شد."
             : message == ManageMessageId.SetTwoFactorSuccess ? "احراز هویت دو مرحله ای بازنشانی شد."
             : message == ManageMessageId.Error ? "یک خطا رخ داده است."
             : "";

            ViewData["RecommendedProducts"] = (_productService.GetSuggestionProductsForce(9));

            return View();
        }

        [Route("UserProfile")]
        [HttpGet]
        public virtual async Task<ActionResult> UserProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Mobile = user.Mobile;
                ViewBag.Address = user.Address;
                ViewBag.PostalCode = user.PostalCode;
            }
            return View();
        }

        [Route("UserProfile")]
        [HttpPost]
        public virtual async Task<ActionResult> UserProfile
            ([Bind(Include = "ID,FirstName,LastName,Mobile,Address,PostalCode")]ProfileViewmodel userprofile)
        {
            if (!ModelState.IsValid)
            {
                return View(userprofile);
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {

                user.FirstName = userprofile.FirstName;
                user.LastName = userprofile.LastName;
                user.Mobile = userprofile.Mobile;
                user.Address = userprofile.Address;
                user.PostalCode = userprofile.PostalCode;

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveAllChangesAsync();

            }
            else
            {
                return View(userprofile);
            }


            return RedirectToAction("Index", new { Message = ManageMessageId.UserProfileSuccessfully });
        }



    }
}
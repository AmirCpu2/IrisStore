﻿using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    public class ProfileViewmodel
    {
        public int? Id { get; set; }

        [Required (ErrorMessage = "نام را وارد نمائید")]
        [Display(Name = "نام ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی را وارد نمائید")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "تلفن همراه وارد نمائید")]
        [StringLength(11, ErrorMessage = "شماره را بدرستی وارد نمائید")]
        [Display(Name = "تلفن همراه")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "آدرس را وارد نمائید")]
        [Display(Name = "آدرس کامل")]
        public string Address { get; set; }

        [Required(ErrorMessage = "کدپستی را وارد نمائید")]
        [Display(Name = "کد پستی")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "آدرس صندوق الکترونیکی را وارد نمائید")]
        [Display(Name = "آدرس صندوق الکترونیکی")]
        public string Email { get; set; }

        public string LatLong { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual string ThumbnailUrl { get; set; }

    }
}
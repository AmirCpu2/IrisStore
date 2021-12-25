using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Areas.CommentManager.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("CommentManager", AreaPrefix = "CommentManager-Admin")]
    public partial class AdminController : Controller
    {
        // GET: CommentManager/Admin
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
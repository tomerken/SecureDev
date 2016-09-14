using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class ShoppingCertController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            List<ShoppingCert> sc = (List<ShoppingCert>)Session["ShoppingCert"];
            ViewBag.MyList = sc;
            return View();
        }



    }
}

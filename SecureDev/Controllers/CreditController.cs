using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class CreditController : BaseController
    {
        // GET: Shop
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}
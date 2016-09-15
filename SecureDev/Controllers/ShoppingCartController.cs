using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class ShoppingCartController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            if(Session["Cart"] == null)
            {
                ViewBag.Message = "No items in cart";
                return View();
            }

            else
            {
                List<CartItem> list = (List<CartItem>)Session["Cart"];
                return View(list);
            }
        }
    }
}

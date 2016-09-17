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
            {
                Logging.Log("Shopping Cart page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            if(Session["Cart"] == null)
            {
                ViewBag.Message = "No items in cart";
                Logging.Log("Successful login to the shoppingcart page", Logging.AccessType.Valid);
                return View();
            }

            else
            {
                List<CartItem> list = (List<CartItem>)Session["Cart"];
                Logging.Log("Successful login to the shoppingcart page", Logging.AccessType.Valid);
                return View(list);
            }
        }
    }
}

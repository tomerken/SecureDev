using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class ShoppingCartController : BaseController
    {
        // The shopping cart page
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

        // Remove an item from the shopping cart
        [HttpGet]
        public ActionResult RemoveItem(String petId)
        {
            string petIDXSS = AntiXssEncoder.HtmlEncode(petId, true);
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Shopping Cart page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            int _petId;
            List<CartItem> list = (List<CartItem>)Session["Cart"];

            try
            {
                _petId = Int32.Parse(petIDXSS);
            }catch{
                Logging.Log("ShopController Remove item : attempt to remove item with non numeric type", Logging.AccessType.Invalid);
                return RedirectToAction("Index");
            }

            foreach(CartItem currCartItem in list)
            {
                if(currCartItem.petId == _petId)
                {
                    list.Remove(currCartItem);
                    Logging.Log("ShopController Remove item : removed item", Logging.AccessType.Valid);
                    if (list.Count != 0)
                        Session["Cart"] = list;
                    else
                        Session["Cart"] = null;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}

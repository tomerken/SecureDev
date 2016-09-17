using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security.AntiXss;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class ShopController : BaseController
    {
        // GET: Stage1
        public ActionResult Stage1()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Shop page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            // the request came from addtocart
            if (TempData["AddToCartMessage"] != null)
            {
                ViewBag.AddToCartMessage = TempData["AddToCartMessage"].ToString();
            }
            var selectBoxs = new ShoppingCart();
            List<SelectListItem> petTypeList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select distinct petType from tblpets", m_dbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        petTypeList.Add(new SelectListItem() { Text = reader.GetString(0).Trim(), Value = reader.GetString(0).Trim()});
                    }
                }

            }

            selectBoxs.PetType = petTypeList;
            selectBoxs.PetName = new[] { new SelectListItem { Value = "", Text = "" } };
            Logging.Log("Successful login to shop page", Logging.AccessType.Valid);
            return View(selectBoxs);
        }

       
        [HttpGet]
        public ActionResult GetStage1PetNames(string petType)
        {
            string petTypeXSS = AntiXssEncoder.HtmlEncode(petType, true);
            List<SelectListItem> petNameList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;

            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petName from tblpets where petType = @petType", m_dbConnection);
                command.Parameters.AddWithValue("@petType", petTypeXSS);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        petNameList.Add(new SelectListItem() { Text = reader.GetString(0).Trim(), Value = reader.GetString(0).Trim() });
                    }
                }

                return Json(petNameList, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetStage1PetPrice(string petName)
        {
            string petNameXSS = AntiXssEncoder.HtmlEncode(petName, true);
            string price = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select price from tblpets where petName = @petName", m_dbConnection);
                command.Parameters.AddWithValue("@petName", petNameXSS);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = reader.GetDecimal(0).ToString();
                    }
                }

                return Json(price, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddToCart(FormCollection form)
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Add to cart in shop page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            string g = form["selectpetname"];
            string v = form["selectpettype"];

            if (form["selectpetname"] == null || form["selectpettype"] == null || form["selectpetname"].ToString() == "" || form["selectpettype"].ToString() == "")
            {
                Logging.Log("POST Add to cart : the user " + Session["LoggedUserName"].ToString() + " attempted to post without the required params ", Logging.AccessType.Invalid);
                TempData["AddToCartMessage"] = "Please choose a pet type and a pet name before adding to cart";
                return RedirectToAction("Stage1");
            }

            string petType;
            string petName;
            int petId;
            bool success = false;
            // getting the price from the database and not from the form!
            int price;
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petName, petType, price, petId  from tblpets where petName = @name AND petType = @type", m_dbConnection);
                command.Parameters.AddWithValue("name", form["selectpetname"].ToString());
                command.Parameters.AddWithValue("type", form["selectpettype"].ToString());
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            // selecting petName and petType back from database - if they passed the prepare statement
                            // they will be returned from database correctly and setting success as true
                            success = true;
                            petName = reader.GetString(0);
                            petType = reader.GetString(1);
                            price = reader.GetInt32(2);
                            petId = reader.GetInt32(3);
                            if (Session["Cart"] == null)
                            {
                                Session["Cart"] = new List<CartItem>();
                            }
                             ((List<CartItem>)Session["Cart"]).Add(new CartItem(petId, petName, petType, price));
                            Logging.Log("Successful add to cart", Logging.AccessType.Valid);
                        }
                    }
                    else
                    {
                        Logging.Log("POST Add to cart : an unsuccessful attempt to add to cart with wrong name/type was made by " + Session["LoggedUserName"].ToString(), Logging.AccessType.Valid);
                    }
                }

            }
            // the values posted were actually in database and not some attack
            if(success)
            {
                TempData["AddToCartMessage"] = "Successfuly added the item to cart";
                return RedirectToAction("Stage1");
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Order confirmation page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            List<CartItem> list = (List<CartItem>)Session["Cart"];
            if(list == null)
            {
                ViewBag.Message = "No items in cart";
                return View();
            }
            return View(list);
        }

        // Post: Confirm
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Buy()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("POST : Buy method", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            List<CartItem> cartItemList = (List<CartItem>)Session["Cart"];
            string userId = Session["LoggedUserID"].ToString();
            if (cartItemList == null)
            {
                ViewBag.Message = "No purchaser";
                return View();
            }

            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("INSERT INTO tbluserPets (userId,petId) VALUES (@userId,@PetId)", m_dbConnection);

                foreach (CartItem currItem in cartItemList)
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@PetId", currItem.petId);
                    command.ExecuteNonQuery();
                }
            }
            Session["Cart"] = null;
            Logging.Log("A successful purchase has been made ", Logging.AccessType.Valid);
            return RedirectToAction("Index", "Information");
        }

    }
}
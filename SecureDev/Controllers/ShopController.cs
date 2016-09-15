using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class ShopController : BaseController
    {
        // GET: Stage1
        public ActionResult Stage1()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

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
            return View(selectBoxs);
        }

       
        [HttpGet]
        public ActionResult GetStage1PetNames(string petType)
        {
            List<SelectListItem> petNameList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;

            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petName from tblpets where petType = @petType", m_dbConnection);
                command.Parameters.AddWithValue("@petType", petType);
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
            string price = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select price from tblpets where petName = @petName", m_dbConnection);
                command.Parameters.AddWithValue("@petName", petName);
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
                return RedirectToAction("Index", "Login");

            string petType;
            string petName;
            bool success = false;
            // getting the price from the database and not from the form!
            int price;
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petName, petType, price from tblpets where petName = @name AND petType = @type", m_dbConnection);
                command.Parameters.AddWithValue("name", form["selectpetname"].ToString());
                command.Parameters.AddWithValue("type", form["selectpettype"].ToString());
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        // selecting petName and petType back from database - if they passed the prepare statement
                        // they will be returned from database correctly and setting success as true
                        success = true;
                        petName = reader.GetString(0);
                        petType = reader.GetString(1);
                        price = reader.GetInt32(2);
                        if (Session["Cart"] == null)
                        {
                            Session["Cart"] = new List<CartItem>();
                        }
                         ((List<CartItem>)Session["Cart"]).Add(new CartItem(petName, petType, price));
                    }
                }

            }
            // the values posted were actually in database and not some attack
            if(success)
            {
                ViewBag.AddToCartMessage = "Successfuly added the item to cart";
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
                return RedirectToAction("Index", "Login");

            List<ShoppingCart> sc = (List<ShoppingCart>)Session["ShoppingCart"];
            ViewBag.MyList = sc;
            return View();
        }

        // GET: Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ShoppingCart sc)
        {
            Console.WriteLine(ViewBag.MyList);
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            
            return View();
        }

    }
}
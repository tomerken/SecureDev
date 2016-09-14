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

            var selectBoxs = new ShoppingCert();
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

        //[HttpGet]
        //public ActionResult GetStage1PetTypes()
        //{

        //}
        
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Stage2(Object o)
        //{

        //    Session["shoppingCert"] = new List<object> { new object(), new object() };
        //    return RedirectToAction("Confirm", "Shop");
        //}
        [HttpPost]
        public ActionResult Stage1(ShoppingCert model)
        {
            //need to do validation
            List<ShoppingCert> sc = new List<ShoppingCert>();
            sc.Add(model);
            sc.Add(model);
            Session["ShoppingCert"] = sc;
            return RedirectToAction("Confirm", "Shop");

        }

        // GET: Stage2
        public ActionResult Confirm()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            List<ShoppingCert> sc = (List<ShoppingCert>)Session["ShoppingCert"];
            ViewBag.MyList = sc;
            return View();
        }

        // GET: Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ShoppingCert sc)
        {
            Console.WriteLine(ViewBag.MyList);
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            
            return View();
        }

    }
}
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
            return View();
        }

        [HttpGet]
        public ActionResult GetStage1PetsTypes()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            List<string> petTypeList = new List<string>();
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select distinct petType from tblPetsLookup", m_dbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        petTypeList.Add(reader.GetString(0).Trim());
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(petTypeList);
                return Content(output);
            }
        }
        
        [HttpGet]
        public ActionResult GetStage1PetsNames(string petName)
        {
            return null;
        }

        [HttpGet]
        public ActionResult GetStage1PetPrice(string price)
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        // GET: Stage2
        public ActionResult Stage2()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            if (Session["Step1"] == null)
                return RedirectToAction("Stage1", "Shop");
            return View();
        }

        // GET: Stage3
        public ActionResult Stage3()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            if (Session["Step1"] == null || Session["Step2"] == null)
                return RedirectToAction("Stage1", "Shop");
            return View();
        }
    }
}
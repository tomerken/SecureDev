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
        public ActionResult GetStage1PetTypes()
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
        public ActionResult GetStage1PetNames(string petType)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            List<string> petNameList = new List<string>();
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petName from tblPetsLookup where petType = @petType", m_dbConnection);
                command.Parameters.AddWithValue("@petType", petType);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        petNameList.Add(reader.GetString(0).Trim());
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(petNameList);
                return Content(output);
            }
        }

        [HttpGet]
        public ActionResult GetStage1PetPrice(string petName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            List<string> petPriceList = new List<string>();
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select petPrice from tblPetsLookup where petName = @petName", m_dbConnection);
                command.Parameters.AddWithValue("@petName", petName);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        petPriceList.Add(reader.GetDecimal(0).ToString());
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(petPriceList);
                return Content(output);
            }
        }

        // GET: Stage2
        public ActionResult Stage2()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            //if (Session["Step1"] == null)
            //    return RedirectToAction("Stage1", "Shop");
            return View();
        }

        // GET: Stage3
        public ActionResult Stage3()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            //if (Session["Step1"] == null || Session["Step2"] == null)
            //    return RedirectToAction("Stage1", "Shop");
            return View();
        }
    }
}
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
    public class InformationController : BaseController
    {
        // Controller for the information page
        // Retrieve the main information page
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Information page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            string petType;
            string petName;
            int Id;
            decimal price;
            List<Information> infoList = new List<Information>(); 
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select u.id, p.petName, p.petType, p.price from tbluserPets up left join tblusers u on up.userId = u.id left join tblpets p on up.petId = p.petId where u.id = @userid", m_dbConnection);
                command.Parameters.AddWithValue("@userId", Session["LoggedUserID"].ToString());
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) { 
                        while (reader.Read())
                        {
                            Id = reader.GetInt32(0);
                            petName = reader.GetString(1);
                            petType = reader.GetString(2);
                            price = reader.GetDecimal(3);
                            
                            infoList.Add(new Information(Id, petName, petType, price));
                        }
                    }
                    else
                    {
                        ViewBag.Message = "No purchase";
                        return View();
                    }
                }
                Logging.Log("Successful login to information page", Logging.AccessType.Valid);
                return View(infoList);
            }
        }
    }
}



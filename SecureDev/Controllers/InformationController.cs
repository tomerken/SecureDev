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
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        [HttpGet]
        //need to get id of user !!!
        public ActionResult GetInfo(User u)
        {
            
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            List<Information> infos = new List<Information>();
            Information info;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("select u.id, u.firstName, u.LastName, p.petName, p.petType from tbluserPets up left join tblusers u on up.userId = u.id left join tblpets p on up.petId = p.petId where u.id = @userid", m_dbConnection);
                command.Parameters.AddWithValue("@userId", 28);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new Information();
                        // adding inforamtion to the list
                        info.ID = reader.GetInt32(0);
                        info.FirstName = reader.GetString(1).Trim();
                        info.LastName = reader.GetString(2).Trim();
                        info.PetName = reader.GetString(3).Trim();
                        info.PetType = reader.GetString(4).Trim();
                        infos.Add(info);
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(infos);
                return Content(output);
            }
        }
    }
}



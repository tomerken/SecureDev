using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;
using System.Web.Script.Serialization;


namespace Vladi2.Controllers
{
    public class HomeController : BaseController
    {
        //entry point for main page as determined in the route config
        public ActionResult Index(string validationError = null)
        {
            var vm = new homeVM() { data = validationError };
            return View(vm);
        }
        //GET: home/login 

   
        public ActionResult XSS(string xss)
        {
            var vm = new homeVM() { data = xss };
            return View(vm);
        }
        //returns the user home page
        public ActionResult UserHome(string userName)
        {
            var vm = new homeVM {data = userName};
            return View(vm);
        }

        [HttpGet]
        public ActionResult Search(String name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            List<Pet> pets = new List<Pet>();
            Pet pet;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblpets WHERE petName LIKE @petName", m_dbConnection);
                command.Parameters.AddWithValue("@petName", "%" + name + "%");
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pet = new Pet();
                        // adding messages to the list
                        pet.PetID = reader.GetInt32(0);
                        pet.PetName = reader.GetString(1).Trim();
                        pet.Price = reader.GetDecimal(2);
                        pet.PetType = reader.GetString(3).Trim();
                        pets.Add(pet);
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(pets);
                return Content(output);
                //Response.Write(output);
                //Response.Flush();
                //Response.End();
            }

            return RedirectToAction("Index");
        }
    }
}
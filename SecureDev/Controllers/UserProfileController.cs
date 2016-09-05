using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class UserProfileController : Controller
    {
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");
            User user = null;
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers WHERE id = @userid", m_dbConnection);
                command.Parameters.AddWithValue("@userid", int.Parse(Session["LoggedUserID"].ToString()));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // adding messages to the list
                        user = new User();
                        user.ID = reader.GetInt32(0);
                        user.Username = reader.GetString(1).Trim();
                        user.FirstName = reader.GetString(3).Trim();
                        user.LastName = reader.GetString(4).Trim();
                        user.Email = reader.GetString(5).Trim();
                        user.Phone = reader.GetString(6).Trim();
                        user.Picture = reader.GetString(7).Trim();
                    }
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User u)
        {
            if (Session["LoggedUserID"] == null)
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}

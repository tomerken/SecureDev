using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginUser u)
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    command.Parameters.AddWithValue("@password", u.Password);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //if we got here - the select succeded , the user exist in db - redirect to userHome page
                            Session["LoggedUserId"] = reader.GetInt32(0);
                            string userName = reader.GetString(1).Trim();
                            Session["LoggedUserName"] = userName;
                            return RedirectToAction("Index", "Home", new { userName });
                        }
                    }
                }
            }
            //the login failed - redirect to login page with the login error
            //return RedirectToAction("Index", "Home", new { validationError = "The username or password are invalid" });
            return View(u);
        }

        public ActionResult Logout()
        {
            if(Session["LoggedUserId"] != null)
            {
                Session["LoggedUserId"] = null;
            }

            if(Session["LoggedUserName"] != null)
            {
                Session["LoggedUserName"] = null;
            }
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}
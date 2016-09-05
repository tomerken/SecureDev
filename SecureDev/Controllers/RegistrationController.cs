using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User u)
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO tblusers (username, password, firstname, lastname, email, phone, picture) VALUES (@username,@password,@firstname,@lastname,@email,@phone,@picture)", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    command.Parameters.AddWithValue("@password", u.Password);
                    command.Parameters.AddWithValue("@firstname", u.FirstName);
                    command.Parameters.AddWithValue("@lastname", u.LastName);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@phone", u.Phone);
                    command.Parameters.AddWithValue("@picture", u.Picture);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return View();
        }
    }
}
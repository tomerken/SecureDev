using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUser u)
        {
            if(ModelState.IsValid)
            {
                var connectionString = string.Format("DataSource={0}", "D:\\SecureDev\\SecureDev\\Sqlite\\db.sqlite");
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    command.Parameters.AddWithValue("@password", u.Password);
                    //using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = '" + u.Username + "' and password = '" + u.Password + "'", m_dbConnection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //if we got here - the select succeded , the user exist in db - redirect to userHome page
                            Session["LoggedUserId"] = reader.GetInt32(0);
                            string userName = reader.GetString(1).Trim();
                            Session["LoggedUserName"] = userName;
                            return RedirectToAction("UserHome", "Home", new { userName });
                        }
                    }
                }
            }
            //the login failed - redirect to login page with the login error
            //return RedirectToAction("Index", "Home", new { validationError = "The username or password are invalid" });
            return View(u);
        }
   
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

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User u)
        {
            if (ModelState.IsValid)
            {
                var connectionString = string.Format("DataSource={0}", "C:\\Users\\tomer\\desktop\\SecureDev\\SecureDev\\SecureDev\\Sqlite\\db.sqlite");
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO tblusers (username, password, firstname, lastname, email, phone, picture) VALUES (@username,@password,@firstname,@lastname,@email,@phone,@picture)", m_dbConnection);
                    command.Parameters.AddWithValue("@username",u.Username);
                    command.Parameters.AddWithValue("@password",u.Password);
                    command.Parameters.AddWithValue("@firstname",u.FirstName);
                    command.Parameters.AddWithValue("@lastname", u.LastName);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@phone", u.Phone);
                    command.Parameters.AddWithValue("@picture", u.Picture);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return View();
        }
    }
}
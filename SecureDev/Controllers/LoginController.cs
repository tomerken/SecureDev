﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class LoginController : BaseController
    {
        // Login page
        // Get the main login page
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // Post a login to the application
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginUser u)
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    string usernameXSS = AntiXssEncoder.HtmlEncode(u.Username, true);
                    string passwordXSS = AntiXssEncoder.HtmlEncode(u.Password, true);
                    command.Parameters.AddWithValue("@username", usernameXSS);
                    command.Parameters.AddWithValue("@password", passwordXSS);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //if we got here - the select succeded , the user exist in db - redirect to userHome page
                            Session["LoggedUserId"] = reader.GetInt32(0);
                            string userName = reader.GetString(1).Trim();
                            Session["LoggedUserName"] = userName;
                            int isAdmin = reader.GetInt32(8);
                            if (isAdmin == 1)
                                Session["IsAdmin"] = 1;
                            Logging.Log("User " + userName + " has logged in to the system", Logging.AccessType.Valid);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            //the login failed - redirect to login page with the login error

            Logging.Log("A failed login has been attempt with the use of " + u.Username + " credentials", Logging.AccessType.Invalid);
            ViewBag.ErrorMessage = "The username or password are invalid";
            return View(u);
        }

        // Logout from the application
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
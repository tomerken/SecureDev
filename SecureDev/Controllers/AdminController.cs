using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class AdminController : BaseController
    {
        // This controller is responsible for the admin page

        // The main admin page - retrieves the users from the databases and their admin status and shows to the user
        public ActionResult Index()
        {
            
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("Admin page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index","Error");
            }
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                if (!checkifAdmin())
                {
                    Logging.Log("Admin page", Logging.AccessType.Unauthorized);
                    return RedirectToAction("Index", "Error");
                }
                Logging.Log("A successful login to the admin page", Logging.AccessType.Valid);
                List<AdminUser> users = new List<AdminUser>();
                AdminUser u;

                SQLiteCommand command = new SQLiteCommand("SELECT id, username, isAdmin FROM tblusers", m_dbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        u = new AdminUser();
                        int id = int.Parse(AntiXssEncoder.HtmlEncode(reader.GetInt32(0).ToString(), true));
                        string username = AntiXssEncoder.HtmlEncode(reader.GetString(1), true);
                        int isAdmin = int.Parse(AntiXssEncoder.HtmlEncode(reader.GetInt32(2).ToString(), true));
                        u.ID = id;
                        u.isAdmin = isAdmin;
                        u.Username = username;
                        users.Add(u);
                    }
                }
                return View(users);
            }
        }
        // This method is reponsible for retreiving admin role details for a specific user id
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("Admin edit page from client address " + Request.UserHostAddress, Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Error");
            }
            string XSSIDstring = AntiXssEncoder.HtmlEncode(id.ToString(), true);
            int XSSID = int.Parse(XSSIDstring);
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                if (!checkifAdmin())
                {
                    Logging.Log("Admin edit page by " + Session["LoggedUserName"].ToString(), Logging.AccessType.Unauthorized);
                    return RedirectToAction("Index", "Error");
                }
                AdminUser u = new AdminUser();

                SQLiteCommand command = new SQLiteCommand("SELECT id, username, isAdmin FROM tblusers WHERE id = @id", m_dbConnection);
                command.Parameters.AddWithValue("@id", XSSID);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            u = new AdminUser();
                            int userId = int.Parse(AntiXssEncoder.HtmlEncode(reader.GetInt32(0).ToString(), true));
                            string username = AntiXssEncoder.HtmlEncode(reader.GetString(1), true);
                            int isAdmin = int.Parse(AntiXssEncoder.HtmlEncode(reader.GetInt32(2).ToString(), true));
                            u.ID = userId;
                            u.isAdmin = isAdmin;
                            u.Username = username;
                        }
                        Logging.Log("A successful login to the admin edit page has been done by " + Session["LoggedUserName"].ToString(), Logging.AccessType.Valid);
                        return View(u);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No such user";
                    }
                    return View();
                }
            }
        }

        // This method is reponsible for posting admin role details for a specific user id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUser u)
        {
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("POST : admin edit page from client address " + Request.UserHostAddress, Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Error");
            }
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                if (!checkifAdmin())
                {
                    Logging.Log("POST : admin edit page by " + Session["LoggedUserName"].ToString(), Logging.AccessType.Unauthorized);
                    return RedirectToAction("Index", "Error");
                }
                SQLiteCommand command = new SQLiteCommand("UPDATE tblusers SET isAdmin = @isAdmin WHERE id = @id", m_dbConnection);
                command.Parameters.AddWithValue("@id", int.Parse(AntiXssEncoder.HtmlEncode(u.ID.ToString(), true)));
                command.Parameters.AddWithValue("@isAdmin", int.Parse(AntiXssEncoder.HtmlEncode(u.isAdmin.ToString(), true)));
                try
                {
                    command.ExecuteNonQuery();
                    Logging.Log("POST : a successful admin edit to the user with id " + u.ID + " changing to " + u.isAdmin + " has been done by " + Session["LoggedUserName"].ToString(), Logging.AccessType.Valid);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    m_dbConnection.Close();
                }
                return RedirectToAction("Index");
            }
        }


        // method for checking if a logged in user is an admin
        private bool checkifAdmin()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT isAdmin FROM tblusers Where username = @username", m_dbConnection);
                command.Parameters.AddWithValue("@username", Session["LoggedUserName"].ToString());
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int isAdmin = reader.GetInt32(0);
                        if (isAdmin != 1)
                            return false;
                        else
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
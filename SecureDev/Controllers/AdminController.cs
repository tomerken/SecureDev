using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] == null)
                return RedirectToAction("Index", "Home");
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
                            return RedirectToAction("Index", "Home");
                    }
                }

                List<AdminUser> users = new List<AdminUser>();
                AdminUser u;

                command = new SQLiteCommand("SELECT id, username, isAdmin FROM tblusers", m_dbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        u = new AdminUser();
                        int id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        int isAdmin = reader.GetInt32(2);
                        u.ID = id;
                        u.isAdmin = isAdmin;
                        u.Username = username;
                        users.Add(u);
                    }
                }
                return View(users);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["LoggedUserName"] == null)
                return RedirectToAction("Index", "Home");
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
                            return RedirectToAction("Index", "Home");
                    }
                }
                AdminUser u = new AdminUser();

                command = new SQLiteCommand("SELECT id, username, isAdmin FROM tblusers WHERE id = @id", m_dbConnection);
                command.Parameters.AddWithValue("@id", id);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            u = new AdminUser();
                            int userId = reader.GetInt32(0);
                            string username = reader.GetString(1);
                            int isAdmin = reader.GetInt32(2);
                            u.ID = userId;
                            u.isAdmin = isAdmin;
                            u.Username = username;
                        }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUser u)
        {
            if (Session["LoggedUserName"] == null)
                return RedirectToAction("Index", "Home");
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
                            return RedirectToAction("Index", "Home");
                    }
                }
                command = new SQLiteCommand("UPDATE tblusers SET isAdmin = @isAdmin WHERE id = @id", m_dbConnection);
                command.Parameters.AddWithValue("@id", u.ID);
                command.Parameters.AddWithValue("@isAdmin", u.isAdmin);
                try
                {
                    command.ExecuteNonQuery();
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
    }
}
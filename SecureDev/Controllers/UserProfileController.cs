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
            ViewData["Message"] = null;
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
                    if (reader.HasRows)
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
                            user.Phone = (!reader.IsDBNull(6)) ? reader.GetString(6).Trim() : string.Empty;
                            user.Picture = (!reader.IsDBNull(7)) ? reader.GetString(7).Trim() : string.Empty;
                        }
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
                SQLiteCommand command;
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    command.Parameters.AddWithValue("@password", u.Password);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            //alert that the password is worng, and throw from this function 

                        }

                    }
                    
                    command = new SQLiteCommand("UPDATE tblusers SET firstname = @firstname, lastname = @lastname, email = @email, phone = @phone, picture = @picture WHERE ID = @id", m_dbConnection);
                    command.Parameters.AddWithValue("@id", int.Parse(Session["LoggedUserId"].ToString()));
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
                    finally
                    {
                        m_dbConnection.Close();
                    }
                }
                ViewData["Message"] = "profile updated Successfully";
                return View();
                //return RedirectToAction("Index","Home");
            }

            return View();
        }
    }
}

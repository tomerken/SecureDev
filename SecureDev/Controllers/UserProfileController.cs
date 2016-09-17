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
    public class UserProfileController : BaseController
    {
        // The user profile page
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("User profile page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
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
                            user.ID = int.Parse(AntiXssEncoder.HtmlEncode(reader.GetInt32(0).ToString(), true));
                            user.Username = AntiXssEncoder.HtmlEncode(reader.GetString(1).Trim(), true);
                            user.FirstName = AntiXssEncoder.HtmlEncode(reader.GetString(3).Trim(), true);
                            user.LastName = AntiXssEncoder.HtmlEncode(reader.GetString(4).Trim(), true);
                            user.Email = AntiXssEncoder.HtmlEncode(reader.GetString(5).Trim(), true);
                            user.Phone = (!reader.IsDBNull(6)) ? AntiXssEncoder.HtmlEncode(reader.GetString(6).Trim(), true) : string.Empty;
                        }
                    }
                }
            }
            Logging.Log("User profile page", Logging.AccessType.Valid);
            return View(user);
        }

        // Change the profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User u)
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("POST : User profile page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                string fileName;
                SQLiteCommand command;
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    string usernameXSS = AntiXssEncoder.HtmlEncode(u.Username, true);
                    string passwordXSS = AntiXssEncoder.HtmlEncode(u.Password, true);
                    command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    command.Parameters.AddWithValue("@username", usernameXSS);
                    command.Parameters.AddWithValue("@password", passwordXSS);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Logging.Log("POST : User profile page tried to edit profile with wrong password " + u.Username, Logging.AccessType.Invalid);
                            ViewBag.Message = "Password is not correct!";
                            ViewBag.Color = "red";
                              
                            return View();
                        }
                    }
                    string sql = "UPDATE tblusers SET firstname = @firstname, lastname = @lastname, email = @email, phone = @phone ";
                    command = new SQLiteCommand();
                    command.Parameters.AddWithValue("@id", int.Parse(Session["LoggedUserId"].ToString()));
                    command.Parameters.AddWithValue("@firstname", AntiXssEncoder.HtmlEncode(u.FirstName, true));
                    command.Parameters.AddWithValue("@lastname", AntiXssEncoder.HtmlEncode(u.LastName, true));
                    command.Parameters.AddWithValue("@email", AntiXssEncoder.HtmlEncode(u.Email, true));
                    command.Parameters.AddWithValue("@phone", AntiXssEncoder.HtmlEncode(u.Phone, true));

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0 && Request.Files[0].FileName != "")
                    {
                        HttpPostedFileBase pictureFile = Request.Files["Picture"];
                        if (pictureFile.ContentLength > 3 * 1024 * 1024)
                        {
                            Logging.Log("POST : User profile page tried to upload a file over 3MB " + u.Username, Logging.AccessType.Invalid);
                            ViewBag.PictureMessage = "File size must not exceed 3MB";
                            return View();
                        }

                        string extension = System.IO.Path.GetExtension(pictureFile.FileName);
                        if (extension != ".jpeg" && extension != ".jpg" && extension != ".bmp" && extension != ".gif" && extension != ".tif" && extension != ".png")
                        {
                            Logging.Log("POST : User profile page tried to upload an image with an invalid file extension", Logging.AccessType.Invalid);
                            ViewBag.PictureMessage = "Please upload a valid file";
                            return View();
                        }
                        string guid = Guid.NewGuid().ToString();
                        fileName = System.IO.Path.Combine(@"D:\SecureDev\SecureDev_Resources\images\profile\", guid + extension);
                        pictureFile.SaveAs(fileName);
                        fileName = guid + extension;
                        sql += ", picture = @picture";
                        command.Parameters.AddWithValue("@picture", fileName);
                    }
                    sql += " WHERE ID = @id";
                    command.Connection = m_dbConnection;
                    command.CommandText = sql;
                    try
                    {
                        command.ExecuteNonQuery();
                        Logging.Log("POST : Successfuly updated user profile page", Logging.AccessType.Valid);
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
                ViewBag.Message = "profile updated Successfully";
                ViewBag.Color = "green";
                return View();
            }
            return View();
        }
        // Change the password page
        public ActionResult ChangePassword()
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("Password change page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // Post a new password for the user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword cp)
        {
            if (Session["LoggedUserID"] == null)
            {
                Logging.Log("POST : Password change page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                SQLiteCommand command;
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username and password = @password", m_dbConnection);
                    command.Parameters.AddWithValue("@username", Session["LoggedUserName"].ToString());
                    command.Parameters.AddWithValue("@password", AntiXssEncoder.HtmlEncode(cp.OldPassword, true));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Logging.Log("POST : Password change page tried to change password with invalid old password", Logging.AccessType.Anonymous);
                            ViewBag.Message = "Password is not correct!";
                            return View();
                        }
                    }
                    string sql = "UPDATE tblusers SET password = @password WHERE id = @id";
                    command = new SQLiteCommand();
                    command.Parameters.AddWithValue("@id", int.Parse(Session["LoggedUserId"].ToString()));
                    command.Parameters.AddWithValue("@password", AntiXssEncoder.HtmlEncode(cp.Password, true));
                    command.Connection = m_dbConnection;
                    command.CommandText = sql;
                    try
                    {
                        command.ExecuteNonQuery();
                        Logging.Log("POST : Password change page successfuly changed password", Logging.AccessType.Valid);
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
                ViewBag.Message = "Password updated successfully";
                return View();
            }
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;

namespace Vladi2.Controllers
{
    public class RegistrationController : BaseController
    {
        // Registration Page
        // Get the main registration page
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // Post a new registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User u)
        {
            if (Session["LoggedUserName"] != null)
            {
                Logging.Log("POST REGISTER PAGE : attempt to register user while already logged in", Logging.AccessType.Invalid);
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Logging.Log("REGISTER PAGE : Attempt to register user with the same username " + u.Username, Logging.AccessType.Invalid);
                            ViewBag.UserMessage = "Username already exists, please choose a different username";
                            return View();
                        }
                    }
                }
                string fileName = null;
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO tblusers (username, password, firstname, lastname, email, phone, picture, isAdmin) VALUES (@username,@password,@firstname,@lastname,@email,@phone,@picture, 0)", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    command.Parameters.AddWithValue("@password", u.Password);
                    command.Parameters.AddWithValue("@firstname", u.FirstName);
                    command.Parameters.AddWithValue("@lastname", u.LastName);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@phone", u.Phone);

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0 && Request.Files[0].FileName != "")
                    {
                        HttpPostedFileBase pictureFile = Request.Files["Picture"];
                        if (pictureFile.ContentLength > (3 * 1024 * 1024))
                        {
                            Logging.Log("POST : attempt to register user with picture over 3MB " + u.Username, Logging.AccessType.Invalid);
                            ViewBag.PictureMessage = "File size must not exceed 3MB";
                            return View();
                        }
                        
                        string extension = System.IO.Path.GetExtension(pictureFile.FileName);
                        if(extension != ".jpeg" && extension != ".jpg" && extension != ".bmp" && extension != ".gif" && extension != ".tif" && extension != ".png")
                        {
                            Logging.Log("POST : attempt to register user with invalid picture extension " + u.Username, Logging.AccessType.Invalid);
                            ViewBag.PictureMessage = "Please upload a valid file";
                            return View();
                        }
                        string guid = Guid.NewGuid().ToString();
                        string path = @"C:\Users\K2View\SecureDev\SecureDev_Resources\images\profile\";
                        //string path = @"D:\SecureDev\SecureDev_Resources\images\profile\";
                        fileName = System.IO.Path.Combine(path, guid + extension);
                        pictureFile.SaveAs(fileName);
                        fileName = guid + extension;
                    }
                    else
                    {
                        fileName = "default.jpg";
                    }
                    command.Parameters.AddWithValue("@picture", fileName);
                    try
                    {
                        command.ExecuteNonQuery();
                        Logging.Log("POST :Successfuly registered user " + u.Username, Logging.AccessType.Valid);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                return RedirectToAction("Index", "Information");
            }
            return View();
        }
    }
}
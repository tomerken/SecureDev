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
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblusers Where username = @username", m_dbConnection);
                    command.Parameters.AddWithValue("@username", u.Username);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
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
                            ViewBag.PictureMessage = "File size must not exceed 3MB";
                            return View();
                        }
                        
                        string extension = System.IO.Path.GetExtension(pictureFile.FileName);
                        if(extension != ".jpeg" && extension != ".jpg" && extension != ".bmp" && extension != ".gif" && extension != ".tif" && extension != ".png")
                        {
                            ViewBag.PictureMessage = "Please upload a valid file";
                            return View();
                        }
                        string guid = Guid.NewGuid().ToString();
                        fileName = System.IO.Path.Combine(Server.MapPath("~/images/profile"), guid + extension);
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
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
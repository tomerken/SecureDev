using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;
using System.Collections;
using System.Web.Security.AntiXss;

namespace Vladi2.Controllers
{
    public class ForumController : BaseController
    {
        // Controller for Forum Page

        // Retreive the main forum page
        public ActionResult Index()
        {
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("Forum page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            List<ForumMessage> msgs = new List<ForumMessage>();
            ForumMessage msg;
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM tblforum", m_dbConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // adding messages to the list
                        msg = new ForumMessage();
                        msg.MessageID = reader.GetInt32(0);
                        msg.UserID = reader.GetInt32(1);
                        msg.Subject = reader.GetString(2).Trim();
                        msg.Body = reader.GetString(3).Trim();
                        msgs.Add(msg);
                    }
                }
            }
            Logging.Log("Successful login to the forum page", Logging.AccessType.Valid);
            return View(msgs);
        }

        // Retrieve the reate a new forum message page
        public ActionResult Create()
        {
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("Forum create message page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            Logging.Log("Successful login to the create forum page", Logging.AccessType.Valid);
            return View();
        }

        // Post a new forum message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForumMessage msg)
        {
            if (Session["LoggedUserName"] == null)
            {
                Logging.Log("POST : Forum create message page", Logging.AccessType.Anonymous);
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
                {
                    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
                    using (var m_dbConnection = new SQLiteConnection(connectionString))
                    {
                        m_dbConnection.Open();
                        SQLiteCommand command = new SQLiteCommand("INSERT INTO tblforum (userId, subject, body) VALUES (@userId,@subject,@body)", m_dbConnection);
                        command.Parameters.AddWithValue("@userId", int.Parse(Session["LoggedUserId"].ToString()));
                        command.Parameters.AddWithValue("@body", AntiXssEncoder.HtmlEncode(msg.Body, true));
                        command.Parameters.AddWithValue("@subject", AntiXssEncoder.HtmlEncode(msg.Subject, true));
                        try
                        {
                            command.ExecuteNonQuery();
                        Logging.Log("POST : Successful creation of a forum post", Logging.AccessType.Valid);
                    }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            return RedirectToAction("Index");
            }


    }
}
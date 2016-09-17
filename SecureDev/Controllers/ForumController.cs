using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vladi2.Models;
using System.Collections;

namespace Vladi2.Controllers
{
    public class ForumController : BaseController
    {
        // GET: Forum
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
                        command.Parameters.AddWithValue("@body", msg.Body);
                        command.Parameters.AddWithValue("@subject", msg.Subject);
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
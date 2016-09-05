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
    }
}
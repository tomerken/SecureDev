﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class Logging
    {
        // The main logging class
        private Logging() { }
        public enum AccessType { Unauthorized, Anonymous, Valid, Invalid};
        public static void Log(string message, AccessType access)
        {
            //string path= @"C:\Users\K2View\SecureDev\SecureDev_Resources\log.txt";
            string path = @"D:\SecureDev\SecureDev_Resources\log.txt";
            using (System.IO.StreamWriter log = new System.IO.StreamWriter(path, true))
            {
                log.Write(System.DateTime.Now + " ");
                log.WriteLine("The request has been made from client ip address " + HttpContext.Current.Request.UserHostAddress);
                switch (access)
                {
                    case AccessType.Anonymous:
                        log.WriteLine("An anonymous request has been attempted :");
                        log.WriteLine(message);
                        break;
                    case AccessType.Unauthorized:
                        log.WriteLine("An unauthorized request has been attempted by " + System.Web.HttpContext.Current.Session["LoggedUserName"] + " : ");
                        log.WriteLine(message);
                        break;
                    case AccessType.Valid:
                        log.WriteLine("A valid request has been attempted by " + System.Web.HttpContext.Current.Session["LoggedUserName"] + " : ");
                        log.WriteLine(message);
                        break;
                    case AccessType.Invalid:
                        log.WriteLine("An invalid request has been attempted :");
                        log.WriteLine(message);
                        break;
                    default:
                        log.WriteLine(message);
                        break;
                }
            }
                
        }

    }
}
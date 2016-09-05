using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class ForumMessage
    {
        [ScaffoldColumn(false)]
        public int MessageID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        [ScaffoldColumn(false)]
        public int UserID { get; set; }
    }
}
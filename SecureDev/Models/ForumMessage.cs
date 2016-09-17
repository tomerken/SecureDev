using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vladi2.Models
{
    public class ForumMessage
    {
        // A model for a forum message
        [ScaffoldColumn(false)]
        public int MessageID { get; set; }
        [Required(ErrorMessage = "Subject is a required field")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Body is a required field")]
        public string Body { get; set; }
        [ScaffoldColumn(false)]
        public int UserID { get; set; }
    }
}
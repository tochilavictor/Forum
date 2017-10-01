using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ToModerator
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public byte SectionId { get; set; }
    }
}
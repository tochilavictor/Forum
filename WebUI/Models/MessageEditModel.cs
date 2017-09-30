using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class MessageEditModel
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public int ToTopic { get; set; }
        public int ToPage { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
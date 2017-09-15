using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class MessagesViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
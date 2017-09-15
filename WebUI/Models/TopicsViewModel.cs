using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class TopicsViewModel
    {
        public IEnumerable<Topic> Topics { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
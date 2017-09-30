using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Infrastructure
{
    public static class PagingConfig
    {
        private static int messagesPerPage = 2;
        private static int topicsPerPage = 5;
        public static int Messages_per_page
        {
            get { return messagesPerPage; }
            set
            {
                if (value < 1) return;
                messagesPerPage = value;
            }
        }
        public static int Topics_Per_Page
        {
            get { return topicsPerPage; }
            set
            {
                if (value < 1) return;
                topicsPerPage = value;
            }
        }
    }
}
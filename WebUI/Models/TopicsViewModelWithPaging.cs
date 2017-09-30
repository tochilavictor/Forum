using Domain;
using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.Entities;

namespace WebUI.Models
{
    public class TopicsViewModelWithPaging
    {
        public IEnumerable<TopicViewModel> Topics { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public bool UserIsModerator { get; set; }
    }
}
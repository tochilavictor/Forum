using Domain;
using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.Entities;

namespace WebUI.Models
{
    public class MessagesViewModelWithPaging
    {
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public int TopicId { get; set; }
        public bool UserIsModerator { get; set; }
    }
}
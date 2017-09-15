using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TopicController : Controller
    {
        private ITopicRepository repository;
        private int messages_per_page = 2;
        public TopicController(ITopicRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Topic(int id, int page = 1)
        {
            Topic t = repository.GetById(id);
            if (t == null) return HttpNotFound();

            IEnumerable<Message> messages = repository.GetMessagesForTopicOnPage(t, page, messages_per_page);

            ViewBag.SectionName = t.name;

            PagingInfo pi = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = messages_per_page,
                TotalItems = repository.NumberOfMessages(t)
            };

            MessagesViewModel vm = new MessagesViewModel { Messages = messages, PagingInfo = pi };

            return View(vm);
        }
    }
}
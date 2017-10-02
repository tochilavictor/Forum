using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using WebUI.Models;
using Domain.ORMEntities;
using WebUI.Models.Entities;
using WebUI.Infrastructure;
using System.Net;

namespace WebUI.Controllers
{
    public class TopicController : Controller
    {
        private ITopicRepository repository;
        private ISectionRepository sectionsRepository;
        private IUserRepository userRepository;
        public TopicController(ITopicRepository repository,ISectionRepository sectionsRepository,IUserRepository userRepository)
        {
            this.repository = repository;
            this.sectionsRepository = sectionsRepository;
            this.userRepository = userRepository;
        }
        public ActionResult Topic(int id, int page = 1)
        {
            Topic t = repository.GetById(id);
            if (t == null) return View("_Error");
            IEnumerable<MessageViewModel> messages = repository
                .GetMessagesForTopicOnPage(t, page, PagingConfig.Messages_per_page)
                .Select(x=>x.ToViewModel());

            ViewBag.CreatorUsername = t.User.Username;
            ViewBag.CreatorId = t.User.UserId;
            ViewBag.CreationDate = t.Creation_date;

            PagingInfo pi = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PagingConfig.Messages_per_page,
                TotalItems = repository.NumberOfMessages(t)
            };

            bool canModerateMessages = false;
            if (User.Identity.IsAuthenticated)
            {
                canModerateMessages = User.IsInRole("Administrator");
                if (User.IsInRole("Moderator"))
                {
                    User currentUser = userRepository.GetUserByUsername(User.Identity.Name);
                    canModerateMessages = userRepository.IsModeratorOfSection(new User { UserId = currentUser.UserId }, new Section { SectionId = t.SectionId });
                }
            }

            MessagesViewModelWithPaging vm = new MessagesViewModelWithPaging
            {
                Messages = messages,
                PagingInfo = pi,
                TopicName = t.Name,
                TopicDescription = t.Description,
                TopicId = t.TopicId,
                UserIsModerator = canModerateMessages
            };

            return View(vm);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create(byte sectionid)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TopicViewModel topic)
        {

            if (ModelState.IsValid)
            {
                topic.CreatorId = userRepository
                    .GetUserByUsername(User.Identity.Name)
                    .UserId;
                topic.CreationDate = DateTime.Now;
                repository.Add(topic.ToOrm());
                int addedTopicId = repository.Topics
                    .Where(x => x.Name == topic.Name && x.CreatorId == topic.CreatorId)
                    .ToList().OrderBy(x=>x.Creation_date).Last().TopicId;
                return RedirectToRoute(new { controller = "Topic", Action = "Topic" , id = addedTopicId, page =1});
            }
            return View(topic);
        }
        [HttpGet]
        [Authorize(Roles = "Moderator,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = repository.GetById(id.Value);
            if (topic == null)
            {
                return View("_Error");
            }
            if (!User.IsInRole("Administrator"))
            {
                User currentUser = userRepository.GetUserByUsername(User.Identity.Name);
                if (!userRepository.IsModeratorOfSection(currentUser, topic.Section))
                {
                    return RedirectToAction("Login", "Account", null);
                };
            }
            ViewBag.SectionId = new SelectList(sectionsRepository.Sections, "SectionId", "Name", topic.SectionId);
            return View(topic.ToViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TopicViewModel topic)
        {
            if (ModelState.IsValid)
            {
                repository.Update(topic.ToOrm());
                return RedirectToRoute(new { controller = "Section", Action = "Index" });
            }
            ViewBag.Sections = new SelectList(sectionsRepository.Sections, "SectionId", "Name", topic.SectionId);
            return View(topic);
        }
        [HttpGet]
        [Authorize(Roles = "Moderator,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = repository.GetById(id.Value);
            if (topic == null)
            {
                return View("_Error");
            }
            if (!User.IsInRole("Administrator"))
            {
                User currentUser = userRepository.GetUserByUsername(User.Identity.Name);
                if (!userRepository.IsModeratorOfSection(currentUser, topic.Section))
                {
                    return RedirectToAction("Login", "Account", null);
                };
            }
            return View(topic.ToViewModel());
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int? id)
        {
            Topic s = new Topic { TopicId = id.Value };
            repository.Delete(s);
            return RedirectToRoute(new { controller = "Section", Action = "Index" });
        }
        public ActionResult Top()
        {
            var topics = repository.Topics.OrderByDescending(u => u.Messages.Count).Take(5);
            var model = topics.Select(t => new TopTopicsViewModel
            {
                Id = t.TopicId,
                Name = t.Name,
                NumberOfMessages = t.Messages.Count()
            });
            return PartialView(model);
        }
    }
}
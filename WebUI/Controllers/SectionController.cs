using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using WebUI.Models;
using WebUI.Models.Entities;
using Domain.ORMEntities;
using System.Net;
using WebUI.Infrastructure;

namespace WebUI.Controllers
{
    public class SectionController : Controller
    {
        private ISectionRepository sectionRepository;
        private IUserRepository userRepository;
        public SectionController(ISectionRepository sectionRep,IUserRepository userRep)
        {
            sectionRepository = sectionRep;
            userRepository = userRep;
        }
        public ActionResult Index()
        {
            return View(sectionRepository.Sections.ToList().Select(ormsection => ormsection.ToViewModel()));
        }
        public ActionResult Section(byte id, int page = 1)
        {
            Section s = sectionRepository.GetById(id);
            if (s == null) return View("_Error");

            IEnumerable<TopicViewModel> topics = sectionRepository
                .GetTopicsForSectionOnPage(s, page, PagingConfig.Topics_Per_Page)
                .Select(ormtopic=>ormtopic.ToViewModel());

            ViewBag.SectionName = s.Name;
            ViewBag.SectionId = s.SectionId;

            PagingInfo pi = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PagingConfig.Topics_Per_Page,
                TotalItems = sectionRepository.NumberOfTopics(s)
            };

            bool canModerateTopics = false;
            if (User.Identity.IsAuthenticated)
            {
                canModerateTopics = User.IsInRole("Administrator");
                if (User.IsInRole("Moderator"))
                {
                    User currentUser = userRepository.GetUserByUsername(User.Identity.Name);
                    canModerateTopics = userRepository.IsModeratorOfSection(new User { UserId = currentUser.UserId }, new Section { SectionId = id });
                }
            }

            TopicsViewModelWithPaging vm = new TopicsViewModelWithPaging { Topics = topics, PagingInfo = pi, UserIsModerator = canModerateTopics };

            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SectionViewModel sectionvm)
        {
            if (ModelState.IsValid)
            {
                sectionRepository.Add(sectionvm.ToOrm());
                return RedirectToRoute(new { controller = "Section", Action = "Index" });
            }
            return View(sectionvm);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = sectionRepository.GetById(id.Value);
            if (section == null)
            {
                return View("_Error");
            }
            return View(section.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SectionViewModel section)
        {
            if (ModelState.IsValid)
            {
                sectionRepository.Update(section.ToOrm());
                return RedirectToAction("Index");
            }
            return View(section);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = sectionRepository.GetById(id.Value);
            if (section == null)
            {
                return View("_Error");
            }
            return View(section.ToViewModel());
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(byte? id)
        {
            Section s = new Section { SectionId = id.Value };
            sectionRepository.Delete(s);
            return RedirectToRoute(new { controller = "Section", Action = "Index" });
        }

    }
}
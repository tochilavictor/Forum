using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private ISectionRepository repository;
        private int topics_per_page=3;
        public HomeController(ISectionRepository rep)
        {
            repository = rep;
        }
        public ActionResult Index()
        {
            return View(repository.Sections);
        }
        public ActionResult Section(int id,int page = 1)
        {
            Section s = repository.GetById(id);
            if (s == null) return HttpNotFound();

            IEnumerable<Topic> topics = repository.GetTopicsForSectionOnPage(s,page, topics_per_page);

            ViewBag.SectionName = s.name;

            PagingInfo pi = new PagingInfo {
                CurrentPage = page, 
                ItemsPerPage = topics_per_page,
                TotalItems = repository.NumberOfTopics(s) };

            TopicsViewModel vm = new TopicsViewModel { Topics = topics, PagingInfo = pi };

            return View(vm);
        }
    }
}
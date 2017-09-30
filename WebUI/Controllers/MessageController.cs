using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Entities;
using WebUI.Infrastructure;
using Domain.ORMEntities;
using System.Net;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class MessageController : Controller
    {
        private IMessageRepository messageRepository;
        private ITopicRepository topicRepository;
        private IAttachedPictureRepository pictureRepository;
        private IUserRepository userRepository;
        public MessageController(IMessageRepository messageRepository,IAttachedPictureRepository pictureRepository, ITopicRepository topicRepository,IUserRepository userRepository)
        {
            this.messageRepository = messageRepository;
            this.pictureRepository = pictureRepository;
            this.topicRepository = topicRepository;
            this.userRepository = userRepository;
        }
        // GET: Message
        [HttpGet]
        public ActionResult Create(int id,int? parentMessageId)
        {
            return View(new MessageViewModel { TopicId = id });
        }
        [HttpPost]
        public ActionResult Create(MessageViewModel messagevm)
        {
            if (ModelState.IsValid)
            {
                messagevm.Creation_date = DateTime.Now;
                messagevm.UserId = userRepository.GetUserByUsername(User.Identity.Name).UserId;
                messagevm.Contains_pictures = messagevm.Files.FirstOrDefault() != null;
                messageRepository.Add(messagevm.ToOrm());
                if (messagevm.Contains_pictures.Value)
                {
                    foreach (var uploadImage in messagevm.Files)
                    {
                        Attached_Picture attachedPicture = new Attached_Picture();
                        using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                        {
                            attachedPicture.Picture = binaryReader.ReadBytes(uploadImage.ContentLength);
                        }
                        attachedPicture.MessageId = messageRepository.Messages
                            .Where(x => x.Contains_pictures ?? false && x.UserId == messagevm.UserId)
                            .OrderBy(x => x.Creation_date)
                            .ToList().Last().MessageId;
                        attachedPicture.Name = uploadImage.FileName;
                        pictureRepository.Add(attachedPicture);
                    }
                }

                Topic topic = topicRepository.GetById(messagevm.TopicId);
                int toPage = (int)Math.Ceiling((decimal)topicRepository.NumberOfMessages(topic) / PagingConfig.Messages_per_page);
                return RedirectToAction("Topic", "Topic", new { id = messagevm.TopicId, page = toPage });
            }
            return View(messagevm);
        }
        public ActionResult DisplayAttachedPicture(int? id,string name,string extension)
        {
            if (id == null || name == null || name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            string filename = name + "." + extension;
            Attached_Picture picture = pictureRepository.GetByPrimaryKey(id.Value, filename);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return File(picture.Picture, "image/jpeg");
        }
        [HttpGet]
        public ActionResult Edit(int sourceid,int page, long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = messageRepository.GetById(id.Value);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(new MessageEditModel { Id = message.MessageId, Value = message.Value, ToTopic = sourceid, ToPage = page });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MessageEditModel messageEM)
        {
            if (ModelState.IsValid)
            {
                messageRepository.Update(new Message { MessageId = messageEM.Id, Value = messageEM.Value });
                return RedirectToRoute("Topic",new { Action = "Topic",id=messageEM.ToTopic,page=messageEM.ToPage });
            }
            return View(messageEM);
        }
        public ActionResult Delete(int sourceid, int page, long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = messageRepository.GetById(id.Value);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(new MessageEditModel { Id = message.MessageId,
                Value = message.Value,
                ToTopic = sourceid,
                ToPage = page,
                CreationDate = message.Creation_date });
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(MessageEditModel messageEM)
        {
            Message m = new Message { MessageId = messageEM.Id };
            messageRepository.Delete(m);
            if (messageEM.ToPage!=1 && topicRepository.GetMessagesForTopicOnPage(
                new Topic { TopicId = messageEM.ToTopic }, messageEM.ToPage, PagingConfig.Messages_per_page).Count() == 0) messageEM.ToPage--;
            return RedirectToRoute("Topic", new { Action = "Topic", id = messageEM.ToTopic, page = messageEM.ToPage });
        }
    }
}
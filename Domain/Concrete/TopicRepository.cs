using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Abstract;
using Domain.ORMEntities;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class TopicRepository : ITopicRepository
    {
        private DbContext ctx;
        private IMessageRepository messageRepository;
        public TopicRepository(DbContext dbcontext,IMessageRepository messageRepository)
        {
            ctx = dbcontext;
            this.messageRepository = messageRepository;
        }
        public IQueryable<Topic> Topics
        {
            get
            {
                return ctx.Set<Topic>();
            }
        }

        public void Add(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException();
            ctx.Set<Topic>().Add(topic);
            var user = ctx.Set<User>().Single(x => x.UserId == topic.CreatorId);
            ctx.Entry(user).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void Delete(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException();
            var topicToDelete = GetById(topic.TopicId);
            if (topicToDelete == null)
                throw new ArgumentException
                    ($"Topic with Id = {topic.TopicId} does not exists");

            foreach (var message in topicToDelete.Messages.ToList())
            {
                messageRepository.Delete(message);
            }
            ctx.Set<Topic>().Remove(topicToDelete);
            ctx.Entry(topicToDelete).State = EntityState.Deleted;
            ctx.SaveChanges();
        }

        public Topic GetById(int id)
        {
            return ctx.Set<Topic>()
                .Include(x=>x.User)
                .FirstOrDefault(x => x.TopicId == id);
        }

        public Topic GetByPredicate(Expression<Func<Topic, bool>> f)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessagesForTopicOnPage(Topic topic, int page, int pagesize)
        {
            if (topic == null) throw new ArgumentNullException();
            var topicOrm = GetById(topic.TopicId);
            if (topicOrm == null) throw new ArgumentException
                    ($"Topic with id {topic.TopicId} doesn't exists");
            return topicOrm.Messages.OrderBy(x => x.MessageId)
                .Skip((page - 1) * pagesize)
                .Take(pagesize).ToList();
        }

        public int NumberOfMessages(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException();
            var topicOrm = GetById(topic.TopicId);
            if (topicOrm == null)
                throw new ArgumentException
                    ($"Topic with Id = {topic.TopicId} does not exists");
            return topicOrm.Messages.Count();
        }

        public void Update(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException();
            var topicToUpdate = GetById(topic.TopicId);
            if (topicToUpdate == null)
                throw new ArgumentException
                    ($"Topic with Id = {topic.TopicId} does not exists");

            topicToUpdate.Name = topic.Name;
            topicToUpdate.Description = topic.Description;
            topicToUpdate.SectionId = topic.SectionId;

            ctx.Entry(topicToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

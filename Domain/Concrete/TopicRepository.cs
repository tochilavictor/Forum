using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Abstract;

namespace Domain.Concrete
{
    public class TopicRepository : ITopicRepository
    {
        private ForumContext ctx = new ForumContext();
        public IQueryable<Topic> Topics
        {
            get
            {
                return ctx.Topics;
            }
        }

        public Topic GetById(int id)
        {
            return ctx.Topics.FirstOrDefault(x => x.TopicId == id);
        }

        public Topic GetByPredicate(Expression<Func<Topic, bool>> f)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessagesForTopicOnPage(Topic topic, int page, int pagesize)
        {
            if (topic == null) throw new ArgumentNullException();
            return topic.Messages.OrderBy(x => x.MessageId).Skip((page - 1) * pagesize).Take(pagesize);
        }

        public int NumberOfMessages(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException();
            return topic.Messages.Count();
        }
    }
}

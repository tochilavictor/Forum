using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ORMEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class MessageRepository : IMessageRepository
    {
        private DbContext ctx;
        public MessageRepository(DbContext dbcontext)
        {
            ctx = dbcontext;
        }
        public IQueryable<Message> Messages
        {
            get
            {
                return ctx.Set<Message>();
            }
        }

        public void Add(Message message)
        {
            if (message == null) throw new ArgumentNullException();
            ctx.Set<Message>().Add(message);
            var user = ctx.Set<User>().Single(x => x.UserId == message.UserId);
            ctx.Entry(user).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void Delete(Message message)
        {
            if (message == null) throw new ArgumentNullException();
            var messageToDelete = GetById(message.MessageId);
            if (messageToDelete == null)
                throw new ArgumentException
                    ($"Message with Id = {message.MessageId} does not exists");

            //if (topicRepository == null) throw new ArgumentException
            //        ($"Add reference to {nameof(ITopicRepository)} if you want update related connections");

            //foreach (var topic in sectionToDelete.Topics.ToList())
            //{
            //    topicRepository.Delete(topic);
            //}
            ctx.Set<Message>().Remove(messageToDelete);
            ctx.SaveChanges();
        }

        public Message GetById(long id)
        {
            return ctx.Set<Message>()
                .Include(x=>x.User)
                .Include(x=>x.Message2)
                .FirstOrDefault(x => x.MessageId == id);
        }

        public Message GetByPredicate(Expression<Func<Message, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Update(Message message)
        {
            if (message == null) throw new ArgumentNullException();
            var sectionToUpdate = GetById(message.MessageId);
            if (sectionToUpdate == null)
                throw new ArgumentException
                    ($"Section with Id = {message.MessageId} does not exists");

            sectionToUpdate.Value = message.Value ?? sectionToUpdate.Value;
            sectionToUpdate.ParentMessageId = message.ParentMessageId ?? sectionToUpdate.ParentMessageId;
            sectionToUpdate.Last_update = DateTime.Now;

            ctx.Entry(sectionToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

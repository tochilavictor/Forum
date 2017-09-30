using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IMessageRepository
    {
        IQueryable<Message> Messages { get; }
        Message GetById(long id);
        Message GetByPredicate(Expression<Func<Message, bool>> f);
        void Add(Message message);
        void Delete(Message message);
        void Update(Message message);
    }
}

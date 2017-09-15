using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ITopicRepository
    {
        IQueryable<Topic> Topics { get; }
        Topic GetById(int id);
        IEnumerable<Message> GetMessagesForTopicOnPage(Topic topic, int page, int pagesize);
        Topic GetByPredicate(Expression<Func<Topic, bool>> f);
        int NumberOfMessages(Topic topic);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ISectionRepository
    {
        IQueryable <Section> Sections { get; }
        Section GetById(int id);
        IEnumerable<Topic> GetTopicsForSectionOnPage(Section section,int page,int pagesize);
        Section GetByPredicate(Expression<Func<Section, bool>> f);
        int NumberOfTopics(Section section);
    }
}

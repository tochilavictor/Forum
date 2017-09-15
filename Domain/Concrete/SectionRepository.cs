using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Domain.Concrete
{
    public class SectionRepository : ISectionRepository
    {
        private ForumContext ctx = new ForumContext();
        public IQueryable<Section> Sections
        {
            get
            {
                return ctx.Sections;
            }
        }

        public Section GetById(int id)
        {
            return ctx.Sections.FirstOrDefault(x => x.SectionId == id);
        }

        public Section GetByPredicate(Expression<Func<Section, bool>> f)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Topic> GetTopicsForSectionOnPage(Section section, int page, int pagesize)
        {
            if (section == null) throw new ArgumentNullException();
            var query = section.Topics.OrderBy(x => x.TopicId);
            var query2 = query.Skip((page - 1) * pagesize).Take(pagesize);
            return section.Topics.OrderBy(x => x.TopicId).Skip((page-1)*pagesize).Take(pagesize);
        }

        public int NumberOfTopics(Section section)
        {
            if (section == null) throw new ArgumentNullException();
            return section.Topics.Count();
        }
    }
}

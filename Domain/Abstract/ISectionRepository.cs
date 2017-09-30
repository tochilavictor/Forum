using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Abstract
{
    public interface ISectionRepository
    {
        IQueryable<Section> Sections { get; }
        Section GetById(byte id);
        IEnumerable<Topic> GetTopicsForSectionOnPage(Section section, int page, int pagesize);
        Section GetByPredicate(Expression<Func<Section, bool>> f);
        int NumberOfTopics(Section section);
        void Add(Section section);
        void Delete(Section section);
        void Update(Section section);
    }
}

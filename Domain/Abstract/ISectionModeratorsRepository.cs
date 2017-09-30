using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ISectionModeratorsRepository
    {
        IQueryable<SectionModerator> SectionModerators { get; }
        SectionModerator GetByPrimaryKey(byte sectionId,int moderatorId);
        void Add(SectionModerator profile);
        void Delete(SectionModerator profile);
        void Update(SectionModerator profile);
    }
}

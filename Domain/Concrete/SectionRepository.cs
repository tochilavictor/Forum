using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.ORMEntities;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class SectionRepository : ISectionRepository
    {
        private ITopicRepository topicRepository;
        private DbContext ctx;
        public SectionRepository(DbContext dbcontext,ITopicRepository topicRep)
        {
            topicRepository = topicRep;
            ctx = dbcontext;
        }
        public IQueryable<Section> Sections
        {
            get
            {
                return ctx.Set<Section>();
            }
        }

        public void Add(Section section)
        {
            if (section == null) throw new ArgumentNullException();
            ctx.Set<Section>().Add(section);
            ctx.SaveChanges();
        }

        public void Delete(Section section)
        {
            if (section == null) throw new ArgumentNullException();
            var sectionToDelete = GetById(section.SectionId);
            if (sectionToDelete == null)
                throw new ArgumentException
                    ($"Section with Id = {section.SectionId} does not exists");

            if (topicRepository == null) throw new ArgumentException
                    ($"Add reference to {nameof(ITopicRepository)} if you want update related connections");

            foreach (var topic in sectionToDelete.Topics.ToList())
            {
                topicRepository.Delete(topic);
            }
            ctx.Set<Section>().Remove(sectionToDelete);
            ctx.SaveChanges();
        }
        public Section GetById(byte id)
        {
            return ctx.Set<Section>()
                .FirstOrDefault(x => x.SectionId == id);
        }

        public IEnumerable<Topic> GetTopicsForSectionOnPage(Section section, int page, int pagesize)
        {
            if (section == null) throw new ArgumentNullException();
            var sectionOrm = GetById(section.SectionId);
            if (sectionOrm == null) throw new ArgumentException
                    ($"Topic with id {section.SectionId} doesn't exists");
            var query = sectionOrm.Topics.OrderBy(x => x.TopicId).
               Skip((page - 1) * pagesize).Take(pagesize);
            return query.ToList();
        }

        public int NumberOfTopics(Section section)
        {
            if (section == null) throw new ArgumentNullException();
            var sectionOrm = GetById(section.SectionId);
            if (sectionOrm == null)
                throw new ArgumentException
                    ($"Section with Id = {section.SectionId} does not exists");
            return sectionOrm.Topics.Count();
        }
        //????
        public void Update(Section section)
        {
            if (section == null) throw new ArgumentNullException();
            var sectionToUpdate = GetById(section.SectionId);
            if (sectionToUpdate == null)
                throw new ArgumentException
                    ($"Section with Id = {section.SectionId} does not exists");

            sectionToUpdate.Name = section.Name;
            sectionToUpdate.Description = section.Description;

            ctx.Entry(sectionToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
        public Section GetByPredicate(Expression<Func<Section, bool>> f)
        {
            throw new NotImplementedException();
        }
    }
}

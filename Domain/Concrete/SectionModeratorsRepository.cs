using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ORMEntities;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class SectionModeratorsRepository : ISectionModeratorsRepository
    {
        private DbContext ctx;
        public SectionModeratorsRepository(DbContext context)
        {
            ctx = context;
        }
        public IQueryable<SectionModerator> SectionModerators
            => ctx.Set<SectionModerator>();

        public void Add(SectionModerator entry)
        {
            if (entry == null) throw new ArgumentNullException();
            ctx.Set<SectionModerator>().Add(entry);
            ctx.SaveChanges();
        }

        public void Delete(SectionModerator entry)
        {
            if (entry == null) throw new ArgumentNullException();
            var entryToDelete = GetByPrimaryKey(entry.SectionId,entry.UserId);
            if (entryToDelete == null)
                throw new ArgumentException
                    ($"There is no entry for section with id = {entry.SectionId} and moderator id = {entry.UserId}");

            ctx.Set<SectionModerator>().Remove(entryToDelete);
            ctx.SaveChanges();
        }

        public SectionModerator GetByPrimaryKey(byte sectionId, int moderatorId)
        {
            return ctx.Set<SectionModerator>()
                .FirstOrDefault(x => x.SectionId == sectionId && x.UserId == x.UserId);
        }

        public void Update(SectionModerator entry)
        {
            if (entry == null) throw new ArgumentNullException();
            var entryToUpdate = GetByPrimaryKey(entry.SectionId, entry.UserId);
            if (entryToUpdate == null)
                throw new ArgumentException
                    ($"There is no entry for section with id = {entry.SectionId} and moderator id = {entry.UserId}");

            entryToUpdate.DateGranted = entry.DateGranted;

            ctx.Entry(entryToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

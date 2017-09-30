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
    public class AttachedPictureRepository : IAttachedPictureRepository
    {
        private DbContext ctx;
        public AttachedPictureRepository(DbContext context)
        {
            ctx = context;
        }
        public IQueryable<Attached_Picture> Pictures => ctx.Set<Attached_Picture>();

        public void Add(Attached_Picture picture)
        {
            if (picture == null) throw new ArgumentNullException();
            ctx.Set<Attached_Picture>().Add(picture);
            ctx.SaveChanges();
        }

        public void Delete(Attached_Picture picture)
        {
            if (picture == null) throw new ArgumentNullException();
            var pictureToDelete = GetByPrimaryKey(picture.MessageId,picture.Name);
            if (pictureToDelete == null)
                throw new ArgumentException
                    ($"There is no picture with name {picture.Name} in messages with Id = {picture.MessageId}");

            ctx.Set<Attached_Picture>().Remove(pictureToDelete);
            ctx.SaveChanges();
        }

        public Attached_Picture GetByPrimaryKey(long id,string name)
        {
            return ctx.Set<Attached_Picture>()
              .FirstOrDefault(x => x.MessageId == id && x.Name == name);
        }

        public void Update(Attached_Picture picture)
        {
            if (picture == null) throw new ArgumentNullException();
            var pictureToUpdate = GetByPrimaryKey(picture.MessageId,picture.Name);
            if (pictureToUpdate == null)
                throw new ArgumentException
                    ($"There is no picture with name {picture.Name} in messages with Id = {picture.MessageId}");

            pictureToUpdate.Name = picture.Name;
            pictureToUpdate.Picture = picture.Picture;

            ctx.Entry(pictureToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IAttachedPictureRepository
    {
        IQueryable<Attached_Picture> Pictures { get; }
        Attached_Picture GetByPrimaryKey(long id,string name);
        void Add(Attached_Picture picture);
        void Delete(Attached_Picture picture);
        void Update(Attached_Picture picture);
    }
}

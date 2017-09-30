using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IProfileReposiory
    {
        IQueryable<User_additional_info> Profiles { get; }
        User_additional_info GetById(int id);
        void Add(User_additional_info profile);
        void Delete(User_additional_info profile);
        void Update(User_additional_info profile);
    }
}

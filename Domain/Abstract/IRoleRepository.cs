using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role GetById(byte id);
    }
}

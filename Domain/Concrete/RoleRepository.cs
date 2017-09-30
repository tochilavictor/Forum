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
    public class RoleRepository : IRoleRepository
    {
        private DbContext ctx;
        public RoleRepository(DbContext context)
        {
            ctx = context;
        }
        public IEnumerable<Role> GetAllRoles()
        {
            return ctx.Set<Role>().ToList();
        }

        public Role GetById(byte id)
        {
            return ctx.Set<Role>()
                .FirstOrDefault(x => x.RoleId == id);
        }
    }
}

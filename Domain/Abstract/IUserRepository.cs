using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        void CreateUser(User user);
        void UpdateUser(User user,bool updateNavigationPropertirs = false);
        void DeleteUser(User user);
        bool IsModeratorOfSection(User user, Section section);
    }
}

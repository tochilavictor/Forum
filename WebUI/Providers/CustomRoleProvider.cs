using Domain;
using Domain.Abstract;
using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebUI.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public IUserRepository UserRepository
            => (IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository));

        public IRoleRepository RoleRepository
            => (IRoleRepository)DependencyResolver.Current.GetService(typeof(IRoleRepository));

        public override bool IsUserInRole(string email, string roleName)
        {

            User user = UserRepository.GetAllUsers().FirstOrDefault(u => u.E_mail == email);

            if (user == null) return false;

            Role userRole = RoleRepository.GetById(user.RoleId);

            if (userRole != null && userRole.Name == roleName)
            {
                return true;
            }

            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var context = new ForumContext())
            {
                var roles = new string[] { };
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null) return roles;

                var userRole = user.Role;

                if (userRole != null)
                {
                    roles = new string[] { userRole.Name };
                }
                return roles;
            }
        }

        public override void CreateRole(string roleName)
        {
            var newRole = new Role() { Name = roleName };
            using (var context = new ForumContext())
            {
                context.Roles.Add(newRole);
                context.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}
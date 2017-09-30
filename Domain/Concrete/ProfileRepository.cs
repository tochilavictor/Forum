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
    public class ProfileRepository : IProfileReposiory
    {
        private DbContext ctx;
        public ProfileRepository(DbContext dbContext)
        {
            ctx = dbContext;
        }
        public IQueryable<User_additional_info> Profiles => ctx.Set<User_additional_info>();

        public void Add(User_additional_info profile)
        {
            if (profile == null) throw new ArgumentNullException();
            ctx.Set<User_additional_info>().Add(profile);
            ctx.SaveChanges();
        }

        public void Delete(User_additional_info profile)
        {
            if (profile == null) throw new ArgumentNullException();
            var profileToDelete = GetById(profile.UserId);
            if (profileToDelete == null)
                throw new ArgumentException
                    ($"Message with Id = {profile.UserId} does not exists");

            ctx.Set<User_additional_info>().Remove(profileToDelete);
            ctx.SaveChanges();
        }

        public User_additional_info GetById(int id)
        {
            return ctx.Set<User_additional_info>()
                .FirstOrDefault(x => x.UserId == id);
        }

        public void Update(User_additional_info profile)
        {
            if (profile == null) throw new ArgumentNullException();
            var profileToUpdate = GetById(profile.UserId);
            if (profileToUpdate == null)
                throw new ArgumentException
                    ($"Section with Id = {profile.UserId} does not exists");

            profileToUpdate.Firstname = profile.Firstname;
            profileToUpdate.Lastname = profile.Lastname;
            profileToUpdate.Patronymic = profile.Patronymic;
            profileToUpdate.City = profile.City;
            profileToUpdate.Country = profile.Country;
            profileToUpdate.Adress1 = profile.Adress1;
            profileToUpdate.Adress2 = profile.Adress2;
            profileToUpdate.Birthdate = profile.Birthdate;
            profileToUpdate.Phone = profile.Phone;
            profileToUpdate.Describing = profile.Describing;

            ctx.Entry(profileToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

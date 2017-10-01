using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ORMEntities;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Domain.Concrete
{
    public class UserRepository : IUserRepository
    {
        private DbContext ctx;
        private ISectionModeratorsRepository sectionModeratorsRepository;
        private IMessageRepository messageRepository;
        private ITopicRepository topicRepository;
        private IProfileReposiory profileRepository;

        public UserRepository(DbContext context,
            ISectionModeratorsRepository sectionModeratorsRep,
            IMessageRepository messageRep,
            ITopicRepository topicRep,
            IProfileReposiory profileRep)
        {
            ctx = context;
            sectionModeratorsRepository = sectionModeratorsRep;
            messageRepository = messageRep;
            topicRepository = topicRep;
            profileRepository = profileRep;
        }
        public void CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException();
            ctx.Set<User>().Add(user);
            ctx.Set<User_additional_info>().Add(new User_additional_info { UserId = user.UserId });
            ctx.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException();
            var userToDelete = GetUserById(user.UserId);
            if (userToDelete == null)
                throw new ArgumentException
                    ($"User with Id = {user.UserId} does not exists");

            profileRepository.Delete(user.User_additional_info);
            foreach (var message in userToDelete.Messages.ToList())
            {
                messageRepository.Delete(message);
            }
            foreach (var topic in userToDelete.Topics.ToList())
            {
                topicRepository.Delete(topic);
            }
            foreach (var moderatingQuery in userToDelete.SectionModerators.ToList())
            {
                sectionModeratorsRepository.Delete(moderatingQuery);
            }

            ctx.Set<User>().Remove(userToDelete);
            ctx.SaveChanges();
        }

        public IQueryable<User> GetAllUsers()
        {
            return ctx.Set<User>();
        }

        public User GetUserByEmail(string email)
        {
            return ctx.Set<User>()
                .FirstOrDefault(x => x.E_mail == email);
        }

        public User GetUserById(int id)
        {
            return ctx.Set<User>()
                .FirstOrDefault(x => x.UserId == id);
        }

        public User GetUserByUsername(string username)
        {
            return ctx.Set<User>()
                .FirstOrDefault(x => x.Username == username);
        }

        public void UpdateUser(User user, bool updateNavigationProperties = false)
        {
            if (user == null) throw new ArgumentNullException();
            var userToUpdate = GetUserById(user.UserId);
            if (userToUpdate == null)
                throw new ArgumentException
                    ($"User with Id = {user.UserId} does not exists");

            userToUpdate.Image = user.Image ?? userToUpdate.Image;
            userToUpdate.Reputation = user.Reputation ?? userToUpdate.Reputation;
            userToUpdate.Username = user.Username ?? userToUpdate.Username;
            userToUpdate.E_mail = user.E_mail ?? userToUpdate.E_mail;
            if (updateNavigationProperties)
            {
                userToUpdate.RoleId = user.RoleId;
                foreach (Topic topic in user.Topics)
                {
                    Topic topicOrm = topicRepository.GetById(topic.TopicId);
                    if (topicOrm == null) topicRepository.Add(topic);
                    else topicRepository.Update(topic);
                }
                foreach (Message message in user.Messages)
                {
                    Message messageOrm = messageRepository.GetById(message.MessageId);
                    if (messageOrm == null) messageRepository.Add(message);
                    else messageRepository.Update(message);
                }
                foreach (SectionModerator moderationInfoEntry in user.SectionModerators)
                {
                    SectionModerator moderationInfoEntryCurrent = sectionModeratorsRepository
                        .GetByPrimaryKey(moderationInfoEntry.SectionId,moderationInfoEntry.UserId);
                    if (moderationInfoEntryCurrent == null) sectionModeratorsRepository.Add(moderationInfoEntry);
                    else sectionModeratorsRepository.Update(moderationInfoEntry);
                }
            }

            ctx.Entry(userToUpdate).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public bool IsModeratorOfSection(User user, Section section)
        {
            if (user == null || section == null) throw new ArgumentNullException();
            user = GetUserById(user.UserId);
            return user.SectionModerators.Any(x => x.SectionId == section.SectionId);
        }
    }
}

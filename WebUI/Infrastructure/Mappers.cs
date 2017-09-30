using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.Entities;

namespace WebUI.Infrastructure
{
    public static class Mappers
    {
        public static Section ToOrm(this SectionViewModel sectionvm)
        {
            return new Section
            {
                SectionId = sectionvm.Id,
                Description = sectionvm.Description,
                Name = sectionvm.Name
            };
        }
        public static SectionViewModel ToViewModel(this Section section)
        {
            return new SectionViewModel
            {
                Id = section.SectionId,
                Description = section.Description,
                Name = section.Name
            };
        }
        public static Topic ToOrm(this TopicViewModel topicvm)
        {
            return new Topic
            {
                TopicId = topicvm.Id,
                Description = topicvm.Description,
                Name = topicvm.Name,
                Creation_date = topicvm.CreationDate,
                CreatorId = topicvm.CreatorId,
                SectionId = topicvm.SectionId
            };
        }
        public static TopicViewModel ToViewModel(this Topic topic)
        {
            return new TopicViewModel
            {
                Id = topic.TopicId,
                Description = topic.Description,
                Name = topic.Name,
                CreatorId = topic.CreatorId,
                CreationDate = topic.Creation_date,
                SectionId = topic.SectionId,
                CreatorUsername=topic.User?.Username??"unknown"
            };
        }
        public static MessageViewModel ToViewModel(this Message message)
        {
            var messagevm = new MessageViewModel
            {
                Id = message.MessageId,
                Creation_date = message.Creation_date,
                Contains_pictures = message.Contains_pictures,
                ParentMessageId = message.ParentMessageId,
                Value = message.Value,
                Last_update = message.Last_update,
                TopicId = message.TopicId,
                UserId = message.UserId,
                CreatorUsername = message.User.Username
            };
            if (messagevm.Contains_pictures??false)
            {
                messagevm.Filenames = message.Attached_Picture
                    .Select(x=>new FullFileName(x.Name.Substring(0, x.Name.IndexOf('.')),x.Name.Substring(x.Name.IndexOf('.') + 1)));
            }
            return messagevm;
        }
        public static Message ToOrm(this MessageViewModel messagevm)
        {
            return new Message
            {
                MessageId=messagevm.Id,
                Creation_date=messagevm.Creation_date,
                Contains_pictures=messagevm.Contains_pictures,
                ParentMessageId=messagevm.ParentMessageId,
                Value=messagevm.Value,
                Last_update=messagevm.Last_update,
                TopicId=messagevm.TopicId,
                UserId=messagevm.UserId
            };
        }
        public static UserViewModel ToViewModel(this User user)
        {
            var uservm = new UserViewModel
            {
                Id = user.UserId,
                Username = user.Username,
                Email = user.E_mail,
                Reputation = user.Reputation,
                RoleName = user.Role.Name,
                Firstname = user.User_additional_info.Firstname,
                Lastname = user.User_additional_info.Lastname,
                Patronymic = user.User_additional_info.Patronymic,
                Phone = user.User_additional_info.Phone,
                Country = user.User_additional_info.Country,
                City = user.User_additional_info.City,
                Adress1 = user.User_additional_info.Adress1,
                Adress2 = user.User_additional_info.Adress2,
                Birthdate = user.User_additional_info.Birthdate,
                Describing = user.User_additional_info.Describing,
                Image = user.Image
            };
            return uservm;
        }
        public static User_additional_info GetAdditionalInfo(this UserViewModel uservm)
        {
            return new User_additional_info
            {
                UserId = uservm.Id,
                Firstname = uservm.Firstname,
                Lastname = uservm.Lastname,
                Patronymic = uservm.Patronymic,
                Adress1 = uservm.Adress1,
                Adress2 = uservm.Adress2,
                Birthdate = uservm.Birthdate,
                City = uservm.City,
                Describing = uservm.Describing,
                Country = uservm.Country,
                Phone = uservm.Phone,
            };
        }
    }
}
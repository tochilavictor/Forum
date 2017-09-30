using Domain;
using Domain.Abstract;
using Domain.Concrete;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebUI.Infrastructure
{
    public class DependencyModule: NinjectModule
    {
        public override void Load()
        {
            Bind<ISectionRepository>().To<SectionRepository>();
            Bind<ITopicRepository>().To<TopicRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IRoleRepository>().To<RoleRepository>();
            Bind<IMessageRepository>().To<MessageRepository>();
            Bind<IAttachedPictureRepository>().To<AttachedPictureRepository>();
            Bind<ISectionModeratorsRepository>().To<SectionModeratorsRepository>();
            Bind<IProfileReposiory>().To<ProfileRepository>();
            Bind<DbContext>().To<ForumContext>().InSingletonScope();
        }
    }
}
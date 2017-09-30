using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using System.Web.Routing;
using Domain.Abstract;
using Domain.Concrete;
using System.Data.Entity;
using Domain;

namespace WebUI.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            // создание контейнера
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            // получение объекта контроллера из контейнера 
            // используя его тип
            return controllerType == null
              ? null
              : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            ninjectKernel.Bind<ISectionRepository>().To<SectionRepository>();
            ninjectKernel.Bind<ITopicRepository>().To<TopicRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            ninjectKernel.Bind<IRoleRepository>().To<RoleRepository>();
            ninjectKernel.Bind<IMessageRepository>().To<MessageRepository>();
            ninjectKernel.Bind<IAttachedPictureRepository>().To<AttachedPictureRepository>();
            ninjectKernel.Bind<ISectionModeratorsRepository>().To<SectionModeratorsRepository>();
            ninjectKernel.Bind<IProfileReposiory>().To<ProfileRepository>();
            ninjectKernel.Bind<DbContext>().To<ForumContext>().InSingletonScope();
        }
    }
}
using Autofac;

using Publications.Interfaces;
using Publications.Services;

namespace Publications.MVC.Infrastructure.Autofac
{
    public class PublicationServicesModule : Module
    {
        protected override void Load(ContainerBuilder container)
        {
            base.Load(container);

            container.RegisterType<DatabasePublicationManager>()
               .As<IPublicationManager>()
               .InstancePerLifetimeScope();
        }
    }
}

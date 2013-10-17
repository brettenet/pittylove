using System.Web.Http;
using Ninject;
using Ninject.Web.Common;
using PittyLove.FormsAuth.MessageHandlers;
using PittyLove.Model;
using WebApiContrib.IoC.Ninject;

namespace PittyLove.FormsAuth
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IUnitOfWork>().To<EfUnitOfWork>().InRequestScope();
            config.DependencyResolver = new NinjectResolver(kernel);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "FeedDoggy",
              routeTemplate: "api/pitbull/{id}/Meal/{lastFed}",
              defaults: new { controller = "Pitbull", action = "FeedDog" }
          );


            //Add handlers for auth
            //config.MessageHandlers.Add(new BasicAuthenticationHandler());
            //config.MessageHandlers.Add(new ApiKeyHandler());
            //config.MessageHandlers.Add(new TokenHandler());
        }
    }
}
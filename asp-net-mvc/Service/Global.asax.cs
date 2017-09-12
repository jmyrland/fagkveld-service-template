using Service.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Service
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ServiceHelper.RegisterService(new ServiceMetadata {
                Name = "mvc-template",  // This is specified in the manifest
                Port = 51673,           // Check the project settings
                Address = "127.0.0.1",  // Look up your ip/host, replace it with the current value
            });
        }
    }
}

using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using OldSchoolBeats.SignalR;

namespace OldSchoolBeats {
    public class WebApiApplication : System.Web.HttpApplication {
        protected void Application_Start() {


            //GlobalHost.HubPipeline.AddModule(new DiagnosticHubPipeline());

            WebApiConfig.Register();



        }
    }
}
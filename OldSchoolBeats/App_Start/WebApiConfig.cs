using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;
using OldSchoolBeats.DataObjects;
//using OldSchoolBeats.Migrations;
using OldSchoolBeats.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Mobile.Service.Config;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using OldSchoolBeats.Services;
using Autofac;

namespace OldSchoolBeats {
    public static class WebApiConfig {
        public static void Register() {

            //Fire-up SignalR
            SignalRExtensionConfig.Initialize();



            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();



            //Allow only users to use our SignalR-Hub
            options.SetRealtimeAuthorization(AuthorizationLevel.User);


            ConfigBuilder builder = new ConfigBuilder(options, (httpConfig, autofac) => {
                autofac.RegisterInstance(new TableLoggingService()).As<ILoggingService>();

            });


            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(builder);

            //Just a sample, on how to set the required options to get rid
            //of some of the sample page error-messages.
            //You can read more here: http://blogs.msdn.com/b/yaohuang1/archive/2012/10/13/asp-net-web-api-help-page-part-2-providing-custom-samples-on-the-help-page.aspx
            config.SetSampleForType(
                "Currently not used.",
                new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
                typeof(OldSchoolArtist));

            //YOU CAN USE THE RESOURCE BROKER AFTER THE ASSEMBLIES FOR
            //THE MANAGED BACKEND USE THE LATEST WEB-API ASSEMBLIES

            //This is directly taken from the documenation about resource
            //Controllers on GitHub (created by the Azure Mobile Services Team)
            //https://github.com/Azure/azure-mobile-services-resourcebroker
            // Create a custom route mapping the resource type into the URI.
            //var resourcesRoute = config.Routes.CreateRoute(
            //                         routeTemplate: "api/resources/{type}",
            //                         defaults: new { controller = "resources" },
            //                         constraints: null);

            // Insert the ResourcesController route at the top of the collection to avoid conflicting with predefined routes.
            //config.Routes.Insert(0, "Resources", resourcesRoute);


            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new OldSchoolBeatsInitializer());

            //trigger migrations manually
            //var migrator = new DbMigrator(new Configuration());
            //migrator.Update();
        }
    }

    public class OldSchoolBeatsInitializer : DropCreateDatabaseIfModelChanges<OldSchoolBeatsContext> {
        protected override void Seed(OldSchoolBeatsContext context) {
            //List<TodoItem> todoItems = new List<TodoItem>
            //{
            //    new TodoItem { Id = "1", Text = "First item", Complete = false },
            //    new TodoItem { Id = "2", Text = "Second item", Complete = false },
            //};

            //foreach (TodoItem todoItem in todoItems)
            //{
            //    context.Set<TodoItem>().Add(todoItem);
            //}

            base.Seed(context);
        }
    }
}


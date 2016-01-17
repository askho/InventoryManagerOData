using OData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.OData.Batch;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

namespace OData
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpBatchRoute(
                routeName: "WebApiBatch",
                routeTemplate: "$batch",
                batchHandler: new DefaultHttpBatchHandler(GlobalConfiguration.DefaultServer));
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Item>("Items");
            builder.EntitySet<Company>("Companies");
            builder.EntitySet<Inventory>("Inventories");
            builder.EntitySet<Event>("Events");
            builder.EntitySet<User>("Users");
            builder.EntitySet<Member>("Members");
            builder.EntitySet<Phone>("Phones");

            config.Routes.MapODataServiceRoute("odata", "/", builder.GetEdmModel());
            

        }
    }
}

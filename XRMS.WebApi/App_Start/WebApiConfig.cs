using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Csla.Core.FieldManager;
using Csla.CustomFieldData;
using XRMS.Business.Repositories;

namespace XRMS.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            ObjectMappingHelper.Setup();
            PropertyInfoFactory.Factory = new PropertyInformationFactory();

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Linkzor.Tests {
    public static class RouteCollectionExtensions {
        public static void MapMvcAttributeRoutesForTesting(this RouteCollection routeCollection) {
            var controllerTypes = (from type in typeof(RouteCollectionExtensions).Assembly.GetExportedTypes()
                                   where
                                       type != null && type.IsPublic
                                       && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                                       && !type.IsAbstract && typeof(IController).IsAssignableFrom(type)
                                   select type).ToList();

            var attributeRoutingAssembly = typeof(RouteCollectionAttributeRoutingExtensions).Assembly;
            var attributeRoutingMapperType =
                attributeRoutingAssembly.GetType("System.Web.Mvc.Routing.AttributeRoutingMapper");

            var mapAttributeRoutesMethod = attributeRoutingMapperType.GetMethod(
                "MapAttributeRoutes",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
                null);

            mapAttributeRoutesMethod.Invoke(null, new object[] { routeCollection, controllerTypes });
        }
    }
}

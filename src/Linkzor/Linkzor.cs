using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Routing;
using static System.String;

namespace Linkzor {
    public class Linkzor {
        private readonly RequestContext _requestContext;
        private readonly RouteCollection _routeCollection;
        public static string Scheme { get; set; } = Uri.UriSchemeHttp;

        public Linkzor(RequestContext requestContext, RouteCollection routeCollection) {
            _requestContext = requestContext;
            _routeCollection = routeCollection;
        }

        public Linkzor() : this(HttpContext.Current.Request.RequestContext, RouteTable.Routes) {

        }

        public Uri CreateFor<TController>(Expression<Func<TController, object>> expression) {
            var methodCall = GetMethodCallFromExpression(expression.Body);
            var controllerName = GetControllerName(typeof(TController));
            var actionName = methodCall.Method.Name;

            var routeValues = RouteValueProvider.GetRouteValues(methodCall);
            SetControllerRouteValues(routeValues, actionName, controllerName);

            var urlFromRouteCollection = _routeCollection.GetVirtualPath(_requestContext, routeValues);

            if (urlFromRouteCollection == null) {
                throw new NullReferenceException($"Couldn't get route information for controller : {controllerName} action : {actionName}");
            }

            var url = UrlProvider.GetUrl(_requestContext.HttpContext, Scheme, urlFromRouteCollection.VirtualPath);

            return new Uri(url);
        }

        private static void SetControllerRouteValues(RouteValueDictionary routeValues, string actionName, string controllerName) {
            routeValues["action"] = actionName;
            routeValues["controller"] = controllerName;
        }

        private static string GetControllerName(Type controllerType) {
            return controllerType.Name.Replace("Controller", Empty);
        }

        private static MethodCallExpression GetMethodCallFromExpression(Expression expressionBody) {
            var methodCall = expressionBody as MethodCallExpression;

            if (methodCall == null) {
                throw new ArgumentException("The expression must be a method call.");
            }

            return methodCall;
        }
    }
}

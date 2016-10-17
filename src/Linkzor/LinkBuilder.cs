using System;
using System.Linq.Expressions;
using System.Web.Http.Routing;

namespace Linkzor
{
    public class LinkBuilder
    {
        private readonly RouteValueProvider _routeValueProvider;
        private readonly UrlHelper _urlHelper;
        public static string Scheme { get; set; } = "http";

        internal LinkBuilder(RouteValueProvider routeValueProvider, UrlHelper urlHelper)
        {
            _routeValueProvider = routeValueProvider;
            _urlHelper = urlHelper;
        }

        internal LinkBuilder() : this(new RouteValueProvider(), new UrlHelper())
        {
            
        }

        public Uri CreateFor<TController>(Expression<Func<TController, object>> expression)
        {
            var methodCall = GetMethodCallFromExpression(expression.Body);

            var controllerName = GetControllerName(typeof(TController));

            var action = methodCall.Method;
            var routeValues = _routeValueProvider.GetRouteValues(methodCall);

            return null;
        }

        private static string GetControllerName(Type controllerType)
        {
            return controllerType.Name.Replace("Controller", string.Empty);
        }

        private static MethodCallExpression GetMethodCallFromExpression(Expression expressionBody)
        {
            var methodCall = expressionBody as MethodCallExpression;

            if (methodCall == null)
            {
                throw new ArgumentException("The expression must be a method call.");
            }

            return methodCall;
        }
    }
}

using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Routing;

namespace Linkzor
{
    public class RouteValueProvider
    {
        public static RouteValueDictionary GetRouteValues(MethodCallExpression methodCall)
        {
            var action = methodCall.Method;
            var arguments = methodCall.Arguments;

            var routeValues = action.GetParameters()
                                .ToDictionary(parameter => parameter.Name, parameter => ValuateExpression(arguments, parameter));

            return new RouteValueDictionary(routeValues);
        }

        private static object ValuateExpression(ReadOnlyCollection<Expression> arguments, ParameterInfo parameter)
        {
            var argument = arguments[parameter.Position];
            var constantExpression = argument as ConstantExpression;
            
            if (constantExpression != null) return constantExpression.Value;

            var lambdaExpression = Expression.Lambda(argument);

            return lambdaExpression.Compile().DynamicInvoke();
        }
    }
}

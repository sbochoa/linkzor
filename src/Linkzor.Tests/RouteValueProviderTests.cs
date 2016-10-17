using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Linkzor.Tests
{
    [TestFixture]
    public class RouteValueProviderTests
    {
        [Test]
        public void GetRouteValues_WithStringValue_ReturnsRouteValues()
        {
            var message = "pong";
            Expression<Func<Ping, object>> expression = ping => ping.Pong(message);
            var methodCall = (MethodCallExpression)expression.Body;

            var routeValues = RouteValueProvider.GetRouteValues(methodCall);

            routeValues[nameof(message)].ShouldBe(message);
        }

        [Test]
        public void GetRouteValues_WithNullValue_ReturnsRouteValues()
        {
            string message = null;
            Expression<Func<Ping, object>> expression = ping => ping.Pong(message);
            var methodCall = (MethodCallExpression)expression.Body;

            var routeValues = RouteValueProvider.GetRouteValues(methodCall);

            routeValues[nameof(message)].ShouldBeNull();
        }

        [Test]
        public void GetRouteValues_WithStringLambda_ReturnsRouteValues()
        {
            Expression<Func<Ping, object>> expression = ping => ping.Pong(Message());
            var methodCall = (MethodCallExpression)expression.Body;

            var routeValues = RouteValueProvider.GetRouteValues(methodCall);

            routeValues["message"].ShouldBe(Message());
        }

        private static string Message()
        {
            return "pong";
        }
    }

    public class Ping
    {
        public object Pong(string message)
        {
            return message;
        }
    }
}

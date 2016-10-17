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
            var message = "hello";
            Expression<Func<Ping, object>> expression = ping => ping.Pong(message);
            var methodCall = (MethodCallExpression)expression.Body;
            var sut = new RouteValueProvider();

            var routeValues = sut.GetRouteValues(methodCall);

            routeValues[nameof(message)].ShouldBe(message);
        }

        [Test]
        public void GetRouteValues_WithNullValue_ReturnsRouteValues()
        {
            string message = null;
            Expression<Func<Ping, object>> expression = ping => ping.Pong(message);
            var methodCall = (MethodCallExpression)expression.Body;
            var sut = new RouteValueProvider();

            var routeValues = sut.GetRouteValues(methodCall);

            routeValues[nameof(message)].ShouldBe(message);
        }

        [Test]
        public void GetRouteValues_WithStringLambda_ReturnsRouteValues()
        {
            Expression<Func<Ping, object>> expression = ping => ping.Pong(Message());
            var methodCall = (MethodCallExpression)expression.Body;
            var sut = new RouteValueProvider();

            var routeValues = sut.GetRouteValues(methodCall);

            routeValues["message"].ShouldBe(Message());
        }

        private string Message()
        {
            return "hello";
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

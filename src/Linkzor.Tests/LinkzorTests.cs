using System;
using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Shouldly;
using static System.String;

namespace Linkzor.Tests {
    [TestFixture]
    public class LinkzorTests {
        public static string BaseUrl = "http://www.foosite.com";

        [Test]
        public void CreateFor_WithRouteParameter_ReturnsUrl() {
            var linkzor = GetLinkzor();
            const int id = 1;
            var uri = linkzor.CreateFor<FooController>(x => x.FooAction(id));

            uri.ToString().ShouldBe(BaseUrl + $"/fooservice/fooaction/{id}");
        }

        [Test]
        public void CreateFor_WithRouteParameterAndQueryString_ReturnsUrl() {
            var linkzor = GetLinkzor();
            const int id = 1;
            const string queryString = "ping";
            var uri = linkzor.CreateFor<FooController>(x => x.FooActionWithQueryString(id, queryString));

            uri.ToString().ShouldBe(BaseUrl + $"/fooservice/fooactionwithquerystring/{id}?queryString={queryString}");
        }

        [Test]
        public void CreateFor_WithRouteParameterAndCustomObject_ReturnsUrl() {
            var linkzor = GetLinkzor();
            const int id = 1;
            var uri = linkzor.CreateFor<FooController>(x => x.FooActionWithCustomObject(id, null));

            uri.ToString().ShouldBe(BaseUrl + $"/fooservice/fooactionwithcustomobject/{id}");
        }

        [Test]
        public void CreateFor_WithRouteParameterAndRouteName_ReturnsUrl() {
            var linkzor = GetLinkzor();
            const int id = 1;
            var uri = linkzor.CreateFor<FooController>(x => x.FooActionWithRouteName(id));

            uri.ToString().ShouldBe(BaseUrl + $"/fooservice?id={id}");
        }

        private static RequestContext CreateRequestContext(string appPath = null) {
            var httpcontext = GetHttpContext(appPath);
            return new RequestContext(httpcontext, new RouteData());
        }

        private static Linkzor GetLinkzor(string appPath = "/") {
            var routeCollection = new RouteCollection();
            routeCollection.MapMvcAttributeRoutesForTesting();
            return new Linkzor(CreateRequestContext(appPath), routeCollection);
        }

        public static HttpContextBase GetHttpContext(string appPath) {
            var mockHttpContext = new Mock<HttpContextBase>();

            if (!IsNullOrEmpty(appPath)) {
                mockHttpContext.Setup(context => context.Request.RawUrl).Returns(appPath);
                mockHttpContext.Setup(context => context.Request.Path).Returns(appPath);
            }

            mockHttpContext.Setup(context => context.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(path => path);
            mockHttpContext.SetupGet(context => context.Request.Url).Returns(new Uri(BaseUrl));
            return mockHttpContext.Object;
        }
    }
}

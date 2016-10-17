using System.Web.Mvc;

namespace Linkzor.Tests
{
    [RoutePrefix("fooservice")]
    public class FooController : Controller {
        [Route("fooaction/{id}")]
        public ActionResult FooAction(int id) {
            return null;
        }

        [Route("fooactionwithquerystring/{id}")]
        public ActionResult FooActionWithQueryString(int id, string queryString) {
            return null;
        }

        [Route("fooactionwithcustomobject/{id}")]
        public ActionResult FooActionWithCustomObject(int id, object customObject) {
            return null;
        }

        [Route(Name = "fooname")]
        public ActionResult FooActionWithRouteName(int id) {
            return null;
        }
    }
}
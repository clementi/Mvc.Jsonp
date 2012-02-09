namespace Mvc.Jsonp
{
    using System.Text;
    using System.Web.Mvc;

    public abstract class JsonpControllerBase : Controller
    {
        protected internal JsonpResult Jsonp(object data, string callback)
        {
            return this.Jsonp(data, callback, null);
        }

        protected internal JsonpResult Jsonp(object data, string callback, JsonRequestBehavior jsonRequestBehavior)
        {
            return this.Jsonp(data, callback, null, jsonRequestBehavior);
        }

        protected internal JsonpResult Jsonp(object data, string callback, string contentType)
        {
            return this.Jsonp(data, callback, contentType, null);
        }

        protected internal JsonpResult Jsonp(object data, string callback, string contentType, JsonRequestBehavior jsonRequestBehavior)
        {
            return this.Jsonp(data, callback, contentType, null, jsonRequestBehavior);
        }

        protected internal JsonpResult Jsonp(object data, string callback, string contentType, Encoding contentEncoding)
        {
            return this.Jsonp(data, callback, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected internal virtual JsonpResult Jsonp(
            object data, 
            string callback, 
            string contentType, 
            Encoding contentEncoding, 
            JsonRequestBehavior jsonRequestBehavior)
        {
            return new JsonpResult
            {
                Callback = callback,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = jsonRequestBehavior
            };
        }
    }
}
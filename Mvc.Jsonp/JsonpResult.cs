namespace Mvc.Jsonp
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class JsonpResult : JsonResult
    {
        private const string NullCallbackExceptionMessage = "Callback is null.";
        private const string NullContextExceptionMessage = "Context is null.";
        private const string JsonpCallbackFormat = "{0}({1});";
        private const string JsonpContentType = "application/javascript";
        private const string InvalidOperationExceptionMessage = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";
        private const string HttpVerbGet = "get";

    	public string Callback { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(NullContextExceptionMessage);

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && context.HttpContext.Request.HttpMethod.ToLower() == HttpVerbGet)
                throw new InvalidOperationException(InvalidOperationExceptionMessage);

            if (this.Callback == null)
                throw new ArgumentNullException(NullCallbackExceptionMessage);

            if (string.IsNullOrEmpty(this.ContentType))
                this.ContentType = JsonpContentType;

            if (this.ContentEncoding == null)
                this.ContentEncoding = Encoding.UTF8;

            HttpResponseBase response = context.HttpContext.Response;

            if (this.Data != null)
            {
                var jsSerializer = new JavaScriptSerializer();
                response.Write(string.Format(JsonpCallbackFormat, this.Callback, jsSerializer.Serialize(this.Data)));
            }
        }
    }
}
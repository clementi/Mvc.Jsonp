namespace Mvc.Jsonp
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class JsonpResult : JsonResult
    {
        private const string NullCallbackExceptionMessage = "Callback cannot be null.";
        private const string NullContextExceptionMessage = "Context cannot be null.";
        private const string JsonpCallbackFormat = "{0}({1});";
        private const string JsonpContentType = "application/javascript";
        private const string InvalidOperationExceptionMessage = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";
        private const string HttpVerbGet = "get";

        public string Callback { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            CheckContext(context);
            this.CheckRequestBehavior(context);
            this.CheckCallback();

            HttpResponseBase response = context.HttpContext.Response;

            this.SetResponseContentType(response);
            this.SetResponseContentEncoding(response);

            if (this.Data != null)
            {
                var jsSerializer = new JavaScriptSerializer();
                response.Write(string.Format(JsonpCallbackFormat, this.Callback, jsSerializer.Serialize(this.Data)));
            }
        }

        private static void CheckContext(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(NullContextExceptionMessage);
        }

        private void CheckRequestBehavior(ControllerContext context)
        {
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && context.HttpContext.Request.HttpMethod.ToLower() == HttpVerbGet)
                throw new InvalidOperationException(InvalidOperationExceptionMessage);
        }

        private void CheckCallback()
        {
            if (this.Callback == null)
                throw new ArgumentNullException(NullCallbackExceptionMessage);
        }

        private void SetResponseContentType(HttpResponseBase response)
        {
            if (string.IsNullOrEmpty(this.ContentType))
                response.ContentType = JsonpContentType;
            else
                response.ContentType = this.ContentType;
        }

        private void SetResponseContentEncoding(HttpResponseBase response)
        {
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
        }
    }
}
namespace Mvc.Jsonp.Tests
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(JsonpResult))]
    class when_the_callback_is_null
    {
        static JsonpResult result;
        static Exception exception;

        Establish context = () => result = new JsonpResult();

        Because of = () =>
            exception = Catch.Exception(() => result.ExecuteResult(new ControllerContext()));

        It should_fail = () =>
            exception.ShouldBeOfType(typeof(ArgumentNullException));
    }

    [Subject(typeof(JsonpResult))]
    class when_the_context_is_null
    {
        const string CallbackFunctionName = "callback";
        static JsonpResult result;
        static Exception exception;

        Establish context = () =>
            result = new JsonpResult { Callback = CallbackFunctionName };

        Because of = () =>
            exception = Catch.Exception(() => result.ExecuteResult(null));

        It should_fail = () =>
            exception.ShouldBeOfType(typeof(ArgumentNullException));
    }

    [Subject(typeof(JsonpResult))]
    class when_the_data_is_null
    {
        const string CallbackFunctionName = "callback";
        const string JsonpFormat = "{0}({1});";
        static JsonpResult result;
        static Mock<HttpResponseBase> httpResponseMock;
        static Mock<HttpContextBase> httpContextMock;
        static ControllerContext controllerContext;

        Establish context = () =>
        {
            result = new JsonpResult { Callback = CallbackFunctionName };

            httpResponseMock = new Mock<HttpResponseBase>();
            httpResponseMock.Setup(resp => resp.Write(Moq.It.IsAny<string>())).Verifiable();
            
            httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(ctx => ctx.Response).Returns(httpResponseMock.Object).Verifiable();

            controllerContext = new ControllerContext { HttpContext = httpContextMock.Object };
        };

        Because of = () => result.ExecuteResult(controllerContext);

        It should_do_nothing = () =>
        {
            httpContextMock.VerifyGet(ctx => ctx.Response, Times.Once());
            httpResponseMock.Verify(resp => resp.Write(Moq.It.IsAny<string>()), Times.Never());
        };
    }

    [Subject(typeof(JsonpResult))]
    class when_the_data_is_not_null
    {
        const int ArrayLength = 5;
        const string CallbackFunctionName = "callback";
        const string JsonpFormat = "{0}({1});";
        static JsonpResult result;
        static Mock<HttpResponseBase> httpResponseMock;
        static Mock<HttpContextBase> httpContextMock;
        static JavaScriptSerializer jsSerializer;
        static ControllerContext controllerContext;

        Establish context = () =>
        {
            jsSerializer = new JavaScriptSerializer();

            result = new JsonpResult
            {
                Callback = CallbackFunctionName,
                Data = Enumerable.Range(0, ArrayLength).ToArray()
            };
            
            httpResponseMock = new Mock<HttpResponseBase>();
            httpResponseMock.Setup(
                resp => resp.Write(string.Format(JsonpFormat, result.Callback, jsSerializer.Serialize(result.Data)))).Verifiable();

            httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(ctx => ctx.Response).Returns(httpResponseMock.Object).Verifiable();

            controllerContext = new ControllerContext { HttpContext = httpContextMock.Object };
        };

        Because of = () => result.ExecuteResult(controllerContext);

        It should_write_out_the_jsonp = () =>
        {
            httpContextMock.VerifyGet(ctx => ctx.Response, Times.Once());
            httpResponseMock.Verify(
                resp => resp.Write(string.Format(JsonpFormat, result.Callback, jsSerializer.Serialize(result.Data))), Times.Once());
        };
    }
}
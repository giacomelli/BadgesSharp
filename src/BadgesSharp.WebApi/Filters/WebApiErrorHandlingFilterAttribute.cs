using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;

namespace BadgesSharp.Infrastructure.Web.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WebApiErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        #region Public Methods
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">Filter context.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                // To avoid memory leak as described here: http://stackoverflow.com/a/20762570/956886
                if (actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Dispose();
                }

                var exception = actionExecutedContext.Exception;
                var httpResponseException = exception as HttpResponseException;
                int code = 400;
                string message = exception.Message;

                if (null != httpResponseException)
                {
                    HttpResponseMessage responseMessage = httpResponseException.Response;
                    message = responseMessage.ReasonPhrase;
                    code = (int)responseMessage.StatusCode;
                }

// #if DEBUG
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, new { code = code, message = message, stackTrace = exception.StackTrace }, new JsonMediaTypeFormatter());
// #else
//                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, new { code = code, message = message }, new JsonMediaTypeFormatter());
//#endif          
            }
        }
        #endregion
    }
}

using System;
using System.Web.Mvc;
using System.Net;


namespace ManagedFusion.Web.Mvc
{
	public class RateLimitedResult : ActionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicAuthenticationResult"/> class.
		/// </summary>
		/// <param name="realm">The realm.</param>
		public RateLimitedResult()
		{
		}

		/// <summary>
		/// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
		/// </summary>
		/// <param name="context">The context within which the result is executed.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			// 401 is the HTTP status code for unauthorized access - setting this
			// will cause the active authentication module to execute its default
			// unauthorized handler
			var response = context.HttpContext.Response;

			response.Clear();
			response.StatusCode = (int)HttpStatusCode.RequestTimeout;
			response.StatusDescription = "Request Timeout";

			response.End();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace ManagedFusion.Web.Mvc
{
	public class HttpBasicAuthenticationResult : ActionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicAuthenticationResult"/> class.
		/// </summary>
		/// <param name="realm">The realm.</param>
		public HttpBasicAuthenticationResult(string realm)
		{
			Realm = realm;
		}

		/// <summary>
		/// Gets or sets the realm.
		/// </summary>
		/// <value>The realm.</value>
		public string Realm { get; set; }

		/// <summary>
		/// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
		/// </summary>
		/// <param name="context">The context within which the result is executed.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			// 401 is the HTTP status code for unauthorized access - setting this
			// will cause the active authentication module to execute its default
			// unauthorized handler
			var response = context.HttpContext.Response;

			response.Clear();
			response.StatusCode = (int)HttpStatusCode.Unauthorized;
			response.StatusDescription = "Unauthorized";
			response.AppendHeader("WWW-Authenticate", "Basic realm=\"" + Realm + "\"");
			response.ContentType = "text/html";

			response.Write("<html><head><title>Unauthorized</title></head><h1>Unauthorized</h1></html>");

			response.Flush(); // TODO: remove this when you figure out a way to get around forms
			response.End();
		}
	}
}
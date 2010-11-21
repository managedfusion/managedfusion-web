using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;
using System.Collections.ObjectModel;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodNotAllowedResult : ActionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodNotAllowedResult"/> class.
		/// </summary>
		/// <param name="httpMethods">The HTTP methods.</param>
		public MethodNotAllowedResult(IList<string> httpMethods)
		{
			SupportedHttpMethods = new ReadOnlyCollection<string>(httpMethods.Select(verb => verb.ToUpperInvariant()).ToList());
		}

		/// <summary>
		/// Gets or sets the supported HTTP methods.
		/// </summary>
		/// <value>The supported HTTP methods.</value>
		public IList<string> SupportedHttpMethods { get; private set; }

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.HttpContext.Response;

			response.Clear();
			response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
			response.StatusDescription = "Method Not Allowed";
			response.AppendHeader("Allow", String.Join(", ", this.SupportedHttpMethods.ToArray()));
			response.ContentType = "text/html";
			response.Write("<html><head><title>Method Not Allowed</title></head><h1>Method Not Allowed</h1><p>Only " + String.Join(", ", this.SupportedHttpMethods.ToArray()) + " is allowed.</p></html>");
			response.Flush();
			response.End();
		}
	}
}

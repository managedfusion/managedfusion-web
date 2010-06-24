using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class UnsupportedMediaTypeResult : ActionResult
	{
		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.Clear();
			context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
			context.HttpContext.Response.StatusDescription = "Unsupported Media Type";
			context.HttpContext.Response.ContentType = "text/html";
			context.HttpContext.Response.Write("<html><head><title>Unsupported Media Type</title></head><h1>Unsupported Media Type</h1><p>HTML is not allowed to be redered by this request.</p></html>");
			context.HttpContext.Response.Flush();
			context.HttpContext.Response.End();
		}
	}
}

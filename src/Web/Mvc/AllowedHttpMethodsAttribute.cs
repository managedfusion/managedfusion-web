using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class AllowedHttpMethodsAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AllowedHttpMethodsAttribute"/> class.
		/// </summary>
		public AllowedHttpMethodsAttribute()
			: this("OPTIONS", "GET", "HEAD", "POST", "PUT", "DELETE", "TRACE", "CONNECT") { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AllowedHttpMethodsAttribute"/> class.
		/// </summary>
		/// <param name="httpMethods">The HTTP methods.</param>
		public AllowedHttpMethodsAttribute(params string[] httpMethods)
		{
			SupportedHttpMethods = new ReadOnlyCollection<string>(httpMethods.Select(verb => verb.ToUpperInvariant()).ToList());
		}

		/// <summary>
		/// Gets or sets the supported HTTP methods.
		/// </summary>
		/// <value>The supported HTTP methods.</value>
		public IList<string> SupportedHttpMethods { get; private set; }

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter filterContext.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!SupportedHttpMethods.Contains(filterContext.HttpContext.Request.HttpMethod.ToUpper()))
			{
				filterContext.Result = new MethodNotAllowedResult(SupportedHttpMethods);
			}
		}
	}
}

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
	public class ServiceOnlyAttribute : ServiceAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceOnlyAttribute"/> class.
		/// </summary>
		public ServiceOnlyAttribute()
		{
			DefaultResponseType = ResponseType.Json;
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter filterContext.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (!filterContext.RouteData.Values.ContainsKey("responseType")
				|| (ResponseType)filterContext.RouteData.Values["responseType"] == ResponseType.Html)
			{
				filterContext.Result = new UnsupportedMediaTypeResult();
			}
		}
	}
}

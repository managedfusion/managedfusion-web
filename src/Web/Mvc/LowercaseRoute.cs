using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ManagedFusion.Web.Mvc
{
	public class LowercaseRoute : System.Web.Routing.Route
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LowercaseRoute"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="routeHandler">The route handler.</param>
		public LowercaseRoute(string url, IRouteHandler routeHandler) 
			: base(url, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LowercaseRoute"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="defaults">The defaults.</param>
		/// <param name="routeHandler">The route handler.</param>
		public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) 
			: base(url, defaults, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LowercaseRoute"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="defaults">The defaults.</param>
		/// <param name="constraints">The constraints.</param>
		/// <param name="routeHandler">The route handler.</param>
		public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) 
			: base(url, defaults, constraints, routeHandler) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LowercaseRoute"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="defaults">The defaults.</param>
		/// <param name="constraints">The constraints.</param>
		/// <param name="dataTokens">The data tokens.</param>
		/// <param name="routeHandler">The route handler.</param>
		public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) 
			: base(url, defaults, constraints, dataTokens, routeHandler) { }

		/// <summary>
		/// Gets the virtual path.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			VirtualPathData data = base.GetVirtualPath(requestContext, values);

			if (data != null)
				data.VirtualPath = data.VirtualPath.ToLowerInvariant();

			return data;
		}
	}
}

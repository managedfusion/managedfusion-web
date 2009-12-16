using System;
using System.Web.Mvc.Resources;
using System.Web.Routing;

namespace System.Web.Mvc
{
	/// <summary>
	/// Represents a result that performs a redirection given some values dictionary
	/// </summary>
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CodedRedirectToRouteResult : ActionResult
	{
		private RouteCollection _routes;

		/// <summary>
		/// Initializes a new instance of the <see cref="ActionRedirectCodedResult"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="responseCode">The response code.</param>
		public CodedRedirectToRouteResult(RouteValueDictionary values, int responseCode)
			: this(null, values, responseCode) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ActionRedirectCodedResult"/> class.
		/// </summary>
		/// <param name="routeName">Name of the route.</param>
		/// <param name="values">The values.</param>
		/// <param name="responseCode">The response code.</param>
		public CodedRedirectToRouteResult(string routeName, RouteValueDictionary values, int responseCode)
		{
			RouteName = routeName ?? String.Empty;
			RouteValues = values ?? new RouteValueDictionary();
			ResponseCode = responseCode;
		}

		/// <summary>
		/// Gets or sets the name of the route.
		/// </summary>
		/// <value>The name of the route.</value>
		public string RouteName
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>The values.</value>
		public RouteValueDictionary RouteValues
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the response code.
		/// </summary>
		/// <value>The response code.</value>
		public int ResponseCode
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the routes.
		/// </summary>
		/// <value>The routes.</value>
		internal RouteCollection Routes
		{
			get
			{
				if (_routes == null)
				{
					_routes = RouteTable.Routes;
				}
				return _routes;
			}
			set
			{
				_routes = value;
			}
		}

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			VirtualPathData data = this.Routes.GetVirtualPath(context.RequestContext, this.RouteName, this.RouteValues);
			if ((data == null) || String.IsNullOrEmpty(data.VirtualPath))
			{
				throw new InvalidOperationException("No route in the route table matches the supplied values.");
			}

			context.HttpContext.Response.Redirect(ResponseCode, data.VirtualPath);
		}
	}
}

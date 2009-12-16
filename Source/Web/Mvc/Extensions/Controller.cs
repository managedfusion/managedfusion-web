using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;

namespace System.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public static class ControllerExtensions
	{
		/// <summary>
		/// Redirects the specified context.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Instance method for consistency with other helpers.")]
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Response.Redirect() takes its URI as a string parameter.")]
		public static CodedRedirectResult Redirect(this IController controller, int responseCode, string url)
		{
			if (String.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			return new CodedRedirectResult(url, responseCode);
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName)
		{
			return RedirectToAction(controller, responseCode, actionName, (RouteValueDictionary)null);
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName, object values)
		{
			return RedirectToAction(controller, responseCode, actionName, new RouteValueDictionary(values));
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName, RouteValueDictionary values)
		{
			return RedirectToAction(controller, responseCode, actionName, null /* controllerName */, values);
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName, string controllerName)
		{
			return RedirectToAction(controller, responseCode, actionName, controllerName, (RouteValueDictionary)null);
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName, string controllerName, object values)
		{
			return RedirectToAction(controller, responseCode, actionName, controllerName, new RouteValueDictionary(values));
		}

		/// <summary>
		/// Redirects to action.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToAction(this IController controller, int responseCode, string actionName, string controllerName, RouteValueDictionary values)
		{
			if (String.IsNullOrEmpty(actionName))
			{
				throw new ArgumentNullException("actionName");
			}

			RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
			newDict["action"] = actionName;
			if (!String.IsNullOrEmpty(controllerName))
			{
				newDict["controller"] = controllerName;
			}
			return new CodedRedirectToRouteResult(newDict, responseCode);
		}

		/// <summary>
		/// Redirects to route.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToRoute(this IController controller, int responseCode, object values)
		{
			return RedirectToRoute(controller, responseCode, new RouteValueDictionary(values));
		}

		/// <summary>
		/// Redirects to route.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToRoute(this IController controller, int responseCode, RouteValueDictionary values)
		{
			return RedirectToRoute(controller, responseCode, null /* routeName */, values);
		}

		/// <summary>
		/// Redirects to route.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="routeName">Name of the route.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToRoute(this IController controller, int responseCode, string routeName)
		{
			return RedirectToRoute(controller, responseCode, routeName, (RouteValueDictionary)null);
		}

		/// <summary>
		/// Redirects to route.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="routeName">Name of the route.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToRoute(this IController controller, int responseCode, string routeName, object values)
		{
			return RedirectToRoute(controller, responseCode, routeName, new RouteValueDictionary(values));
		}

		/// <summary>
		/// Redirects to route.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="routeName">Name of the route.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public static CodedRedirectToRouteResult RedirectToRoute(this IController controller, int responseCode, string routeName, RouteValueDictionary values)
		{
			RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
			return new CodedRedirectToRouteResult(routeName, newDict, responseCode);
		}
	}
}

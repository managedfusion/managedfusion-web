using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace ManagedFusion.Web.Mvc
{
	public abstract class BaseAuthorizeAttribute : AuthorizeAttribute
	{
		/// <summary>
		/// Gets the authenticate result to return when authorization fails.
		/// </summary>
		/// <value>The authenticate result.</value>
		public virtual ActionResult AuthenticateResult { get { return new HttpUnauthorizedResult(); } }

		/// <summary>
		/// Caches the validate handler.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="data">The data.</param>
		/// <param name="validationStatus">The validation status.</param>
		private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
		{
			validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
		}

		/// <summary>
		/// Called when [authorization].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			if (AuthorizeCore(filterContext.HttpContext))
			{
				// ** IMPORTANT **
				// Since we're performing authorization at the action level, the authorization code runs
				// after the output caching module. In the worst case this could allow an authorized user
				// to cause the page to be cached, then an unauthorized user would later be served the
				// cached page. We work around this by telling proxies not to cache the sensitive page,
				// then we hook our custom authorization code into the caching mechanism so that we have
				// the final say on whether a page should be served from the cache.

				HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
				cachePolicy.SetProxyMaxAge(new TimeSpan(0));
				cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
			}
			else
			{
				// auth failed, redirect to authentication type
				filterContext.Result = AuthenticateResult;
			}
		}
	}
}
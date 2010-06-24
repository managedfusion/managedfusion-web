using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace ManagedFusion.Web.Mvc
{
	public class AuthorizeHostAddressAttribute : AuthorizeAttribute
	{
		public AuthorizeHostAddressAttribute() { }

		public string HostAddress { get; set; }

		#region IAuthorizationFilter Members

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return String.Equals(httpContext.Request.UserHostAddress, HostAddress, StringComparison.InvariantCultureIgnoreCase);
		}

		#endregion
	}
}
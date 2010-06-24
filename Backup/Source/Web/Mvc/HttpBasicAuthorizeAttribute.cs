using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace ManagedFusion.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class HttpBasicAuthorizeAttribute : AuthorizeAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicHttpAuthorizeAttribute"/> class.
		/// </summary>
		public HttpBasicAuthorizeAttribute()
		{
			Realm = "Secured";
			UserNameComparison = StringComparison.InvariantCultureIgnoreCase;
			PasswordComparison = StringComparison.InvariantCulture;
		}

		/// <summary>
		/// Gets or sets the realm.
		/// </summary>
		/// <value>The realm.</value>
		public string Realm { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the user name comparison.
		/// </summary>
		/// <value>The user name comparison.</value>
		public StringComparison UserNameComparison { get; set; }

		/// <summary>
		/// Gets or sets the password comparison.
		/// </summary>
		/// <value>The password comparison.</value>
		public StringComparison PasswordComparison { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new HttpBasicAuthenticationResult(Realm);
		}

		/// <summary>
		/// Authorizes the core.
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		/// <returns>
		/// 	<c>true</c> if authorized; otherwise, <c>false</c>.
		/// </returns>
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var request = httpContext.Request;
			var authorizationHeader = request.Headers["Authorization"];

			if (authorizationHeader == null || !authorizationHeader.StartsWith("Basic "))
				return false;

			var credentials = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(authorizationHeader.Substring(6))).Split(new[] { ':' }, 2);

			if (credentials.Length != 2)
				return false;

			var userName = credentials[0];
			var password = credentials[1];

			return String.Equals(UserName, userName, UserNameComparison) 
				&& String.Equals(Password, password, PasswordComparison);
		}
	}
}

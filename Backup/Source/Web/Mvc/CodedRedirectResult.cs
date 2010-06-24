using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc.Resources;
using System.Web.Routing;

namespace System.Web.Mvc
{
	/// <summary>
	/// Represents a result that performs a redirection given some values dictionary
	/// </summary>
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CodedRedirectResult : ActionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodedRedirectResult"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="responseCode">The response code.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Response.Redirect() takes its URI as a string parameter.")]
		public CodedRedirectResult(string url, int responseCode)
		{
			if (String.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");

			Url = url;
			ResponseCode = responseCode;
		}

		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>The values.</value>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", MessageId = "0#", Justification = "Response.Redirect() takes its URI as a string parameter.")]
		public string Url
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
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			context.HttpContext.Response.Redirect(ResponseCode, Url);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class JavaScriptCallbackResult : SerializedResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JavaScriptResult"/> class.
		/// </summary>
		public JavaScriptCallbackResult()
		{
			ContentType = "text/javascript";
			Callback = "callback";
		}

		/// <summary>
		/// Gets or sets the callback.
		/// </summary>
		/// <value>The callback.</value>
		public string Callback
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "js"; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			return Callback + "(" + Data.Serialize(new JsonResult.JsonSerializer(Data), SerializePublicMembers) + ");";
		}

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{

			Callback = context.HttpContext.Request.QueryString["callback"];

			if (String.IsNullOrEmpty(Callback))
				Callback = "callback";

			base.ExecuteResult(context);
		}
	}
}

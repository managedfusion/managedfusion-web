using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;
using System.IO;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class JavaScriptCallbackResult : JsonView
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
		private string Callback
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
			return Callback + "(" + base.GetContent() + ");";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewContext"></param>
		/// <param name="writer"></param>
		public override void Render(ViewContext viewContext, TextWriter writer)
		{
			Callback = viewContext.HttpContext.Request.QueryString["callback"];

			if (String.IsNullOrEmpty(Callback))
				Callback = "callback";

			base.Render(viewContext, writer);
		}
	}
}

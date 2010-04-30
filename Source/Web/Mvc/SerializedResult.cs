using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

using ManagedFusion.Serialization;
using System.IO;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class SerializedResult : ActionResult, ISerializableActionResult
	{
		public SerializedResult(object model)
			: this(model, new AutoSerializedView()) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult"/> class.
		/// </summary>
		public SerializedResult(object model, SerializedView view)
			: this()
		{
			ModelSerializer = view;
			Model = model;
		}

		protected SerializedResult()
		{
			StatusCode = 200;
			StatusDescription = "OK";
		}

		/// <summary>
		/// Gets or sets the model serializer.
		/// </summary>
		public virtual SerializedView ModelSerializer
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public virtual object Model
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string StatusDescription { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static string NormalizeType(string type)
		{
			if (String.Equals(type, "javascript", StringComparison.InvariantCultureIgnoreCase))
				return "javascript";
			else if (String.Equals(type, "jsonp", StringComparison.InvariantCultureIgnoreCase))
				return "javascript";

			return type;
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public static ResponseType GetResponseType(ControllerContext filterContext)
		{
			string type = NormalizeType(filterContext.HttpContext.Request.QueryString["type"]);
			ResponseType responseType = ResponseType.Html;

			// check to see if we should try to parse it to an enum
			if (!String.IsNullOrEmpty(type))
				responseType = ManagedFusion.Utility.ParseEnum<ResponseType>(type);

			// if the response type is still the default HTML check the Accept header
			// if the requestion is an XMLHttpRequest
			if (responseType == ResponseType.Html && filterContext.HttpContext.Request.AcceptTypes != null)
			{
				foreach (string accept in filterContext.HttpContext.Request.AcceptTypes)
				{
					switch (accept.ToLower())
					{
						case "application/json":
						case "application/x-json": responseType = ResponseType.Json; break;

						case "application/javascript":
						case "application/x-javascript":
						case "text/javascript": responseType = ResponseType.JavaScript; break;

						case "application/xml":
						case "text/xml": responseType = ResponseType.Xml; break;

						case "text/csv": responseType = ResponseType.Csv; break;
					}

					if (responseType != ResponseType.Html)
						break;
				}
			}

			return responseType;
		}

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (ModelSerializer == null || ModelSerializer is AutoSerializedView)
				UpdateModelSerializer(context);

			WriteResponse(context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		private void UpdateModelSerializer(ControllerContext context)
		{
			SerializedView view = ModelSerializer as SerializedView;

			if (view == null)
				view = new AutoSerializedView();

			switch (GetResponseType(context))
			{
				case ResponseType.JavaScript:
					ModelSerializer = new JavaScriptCallbackResult();
					break;

				case ResponseType.Json:
					ModelSerializer = new JsonResult();
					break;

				case ResponseType.Xml:
					ModelSerializer = new XmlResult();
					break;

				case ResponseType.Csv:
					ModelSerializer = new CsvResult();
					break;

				case ResponseType.Html:
				default:
					break;
			}

			if (ModelSerializer is SerializedView && view != null)
			{
				var resultX = (ModelSerializer as SerializedView);

				resultX.FollowFrameworkIgnoreAttributes = view.FollowFrameworkIgnoreAttributes;
				resultX.SerializePublicMembers = view.SerializePublicMembers;

				foreach (var header in view.SerializedHeader)
					resultX.SerializedHeader.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		private void WriteResponse(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			HttpResponseBase response = context.HttpContext.Response;

			ModelSerializer.StatusCode = StatusCode;
			ModelSerializer.StatusDescription = StatusDescription;

			if (Model != null)
				ModelSerializer.Render(new ViewContext(context, ModelSerializer, new ViewDataDictionary(Model), new TempDataDictionary(), response.Output), response.Output);
		}
	}
}
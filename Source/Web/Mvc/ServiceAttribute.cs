using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Net;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceAttribute"/> class.
		/// </summary>
		public ServiceAttribute()
		{
			Order = 0;
			OnlyRespondToAjaxRequests = false;
			ErrorResult = null;
		}

		public bool OnlyRespondToAjaxRequests { get; set; }

		private Type _errorResult;
		public Type ErrorResult
		{
			get
			{
				return _errorResult;
			}
			set
			{
				if (value != null && value.GetInterface("ISerializableErrorResult", false) == null)
					throw new ArgumentException("ErrorResult must be a type with interface ISerializableActionResult.");

				_errorResult = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private string NormalizeType(string type)
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
		public override void OnActionExecuting(ActionExecutingContext filterContext)
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

			if (filterContext.RouteData.Values.ContainsKey("responseType"))
				filterContext.RouteData.Values.Remove("responseType");

			// set the value in the route data so it can be used in the methods
			filterContext.RouteData.Values.Add("responseType", responseType);
		}
		
		/// <summary>
		/// Called when [action executed].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext != null && filterContext.Exception != null && ErrorResult != null)
			{
				ISerializableErrorResult result = Activator.CreateInstance(ErrorResult) as ISerializableErrorResult;
				result.Error = filterContext.Exception.ToString();

				if (filterContext.Exception is HttpException)
				{
					result.StatusCode = (filterContext.Exception as HttpException).GetHttpCode();
					result.StatusDescription = ((HttpStatusCode)result.StatusCode).ToString().FromPascalCase();
				}

				filterContext.Result = result as ActionResult;
				filterContext.ExceptionHandled = true;
			}
			
			if (filterContext.Result is ViewResult)
			{
				ViewResult result = filterContext.Result as ViewResult;
				SerializedView view = result.View as SerializedView;

				switch ((ResponseType)filterContext.RouteData.Values["responseType"])
				{
					case ResponseType.JavaScript:
						result.View = new JavaScriptCallbackResult();
						break;

					case ResponseType.Json:
						result.View = new JsonResult();
						break;

					case ResponseType.Xml:
						result.View = new XmlResult();
						break;

					case ResponseType.Csv:
						result.View = new CsvResult();
						break;

					case ResponseType.Html:
					default:
						break;
				}

				if (result.View is SerializedResult && view != null)
				{
					var resultX = (result.View as SerializedResult);

					resultX.FollowFrameworkIgnoreAttributes = view.FollowFrameworkIgnoreAttributes;
					resultX.SerializePublicMembers = view.SerializePublicMembers;

					foreach (var header in view.SerializedHeader)
						resultX.SerializedHeader.Add(header.Key, header.Value);
				}
			}
			else if (filterContext.Result is ISerializableActionResult)
			{
				ISerializableActionResult result = filterContext.Result as ISerializableActionResult;

				switch ((ResponseType)filterContext.RouteData.Values["responseType"])
				{
					case ResponseType.JavaScript:
						filterContext.Result = new JavaScriptCallbackResult {
							Model = result.Model
						};
						break;

					case ResponseType.Json:
						filterContext.Result = new ManagedFusion.Web.Mvc.JsonResult {
							Model = result.Model
						};
						break;

					case ResponseType.Xml:
						filterContext.Result = new XmlResult {
							Model = result.Model
						};
						break;

					case ResponseType.Csv:
						filterContext.Result = new CsvResult {
							Model = result.Model
						};
						break;

					case ResponseType.Html:
					default:
						filterContext.Result = (ActionResult)result;
						break;
				}
			}
		}
	}
}

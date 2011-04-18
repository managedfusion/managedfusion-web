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
			ExceptionResult = null;
			DefaultResponseType = ResponseType.Html;
		}

		public ResponseType DefaultResponseType { get; set; }

		private Type _exceptionResult;
		public Type ExceptionResult
		{
			get
			{
				return _exceptionResult;
			}
			set
			{
				if (value != null && value.GetInterface("ISerializableExceptionResult", false) == null)
					throw new ArgumentException("ErrorResult must impliment interface ISerializableExceptionResult.");

				_exceptionResult = value;
			}
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var responseType = ServiceHelper.GetResponseType(filterContext, DefaultResponseType);

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
			if (filterContext != null 
				&& filterContext.Exception != null 
				&& ExceptionResult != null)
			{
				var result = Activator.CreateInstance(ExceptionResult) as ISerializableExceptionResult;
				result.ExceptionMessage = filterContext.Exception.Message;
				result.Exception = filterContext.Exception;

				if (filterContext.Exception is HttpException)
				{
					result.ModelSerializer.StatusCode = (filterContext.Exception as HttpException).GetHttpCode();
					result.ModelSerializer.StatusDescription = ((HttpStatusCode)result.ModelSerializer.StatusCode).ToString().FromPascalCase();
				}

				filterContext.Result = result as ActionResult;
				filterContext.ExceptionHandled = true;
			}
			
			if (filterContext.Result is ViewResult)
			{
				var result = filterContext.Result as ViewResult;
				var view = result.View as SerializedView;

				switch ((ResponseType)filterContext.RouteData.Values["responseType"])
				{
					case ResponseType.JavaScript:
						result.View = new JavaScriptCallbackResult();
						break;

					case ResponseType.Json:
						result.View = new JsonView();
						break;

					case ResponseType.Xml:
						result.View = new XmlView();
						break;

					case ResponseType.Csv:
						result.View = new CsvResult();
						break;

					case ResponseType.Html:
					default:
						break;
				}

				if (result.View is SerializedView && view != null)
					ServiceHelper.CopyProperties(view, (SerializedView)result.View);
			}
			else if (filterContext.Result is ISerializableActionResult && !typeof(SerializedResult).IsAssignableFrom(filterContext.Result.GetType()))
			{
				ISerializableActionResult result = filterContext.Result as ISerializableActionResult;

				switch ((ResponseType)filterContext.RouteData.Values["responseType"])
				{
					case ResponseType.JavaScript:
					case ResponseType.Json:
					case ResponseType.Xml:
					case ResponseType.Csv:
						var resultX = new SerializedResult(result.Model);
						ServiceHelper.CopyProperties(result.ModelSerializer, resultX.ModelSerializer);
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

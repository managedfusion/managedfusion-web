using System;
using ManagedFusion.Web.Mvc;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	public class AutoSerializedView : SerializedView
	{
		public AutoSerializedView()
		{
			StatusCode = 200;
			StatusDescription = "OK";
		}

		public override void Render(System.Web.Mvc.ViewContext viewContext, System.IO.TextWriter writer)
		{
			var focus = (IView)null;

			ResponseType responseType = ResponseType.None;
			var routeData = viewContext.RouteData.Values;

			object tempObj;
			if (routeData.TryGetValue("responseType", out tempObj) && tempObj is ResponseType)
				responseType = (ResponseType)tempObj;

			if (responseType == ResponseType.None)
				responseType = ServiceHelper.GetResponseType(viewContext);

			switch (responseType)
			{
				case ResponseType.JavaScript:
					focus = new JavaScriptCallbackResult();
					break;

				case ResponseType.Json:
					focus = new JsonView();
					break;

				case ResponseType.Xml:
					focus = new XmlView();
					break;

				case ResponseType.Csv:
					focus = new CsvResult();
					break;

				case ResponseType.Html:
				default:
					focus = viewContext.View;
					break;
			}

			if (focus is SerializedView)
				ServiceHelper.CopyProperties(this, (SerializedView)focus);

			if (focus != null)
				focus.Render(viewContext, writer);
		}
	}
}

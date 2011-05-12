using System.Web.Mvc;
using System.Diagnostics;

namespace ManagedFusion.Web.Mvc
{
	public class StopwatchAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var stopwatch = new Stopwatch();
			filterContext.HttpContext.Items["Stopwatch"] = stopwatch;

			stopwatch.Start();
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var stopwatch = (Stopwatch)filterContext.HttpContext.Items["Stopwatch"];
			stopwatch.Stop();

			var httpContext = filterContext.HttpContext;
			var response = httpContext.Response;

			response.AddHeader("X-Runtime", stopwatch.Elapsed.ToString());
		}
	}
}

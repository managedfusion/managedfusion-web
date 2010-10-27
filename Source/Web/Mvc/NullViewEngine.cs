using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	internal class NullViewEngine : IViewEngine
	{
		#region IViewEngine Members

		public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
		{
			return null;
		}

		public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
		{
			return null;
		}

		public void ReleaseView(ControllerContext controllerContext, IView view)
		{
		}

		#endregion
	}
}

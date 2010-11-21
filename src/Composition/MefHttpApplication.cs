using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Web;

namespace ManagedFusion.Web.Composition
{
	public class MefHttpApplication : HttpApplication
	{
		private static WebScopedContainerManager _containerManager;

		public MefHttpApplication()
		{
			BeginRequest += (sender, e) => { _containerManager.CreateRequestContainer(HttpContext.Current.Items); };
			EndRequest += (sender, e) => { _containerManager.EndRequestContainer(HttpContext.Current.Items); };
		}

		protected WebScopedContainerManager ContainerManager
		{
			get { return _containerManager; }
		}

		protected virtual void Application_Start()
		{
			if (_containerManager == null)
			{
				var catalog = CreateBaseCatalog();
				_containerManager = new WebScopedContainerManager(catalog);
			}
		}

		protected virtual void Application_End()
		{
			if (_containerManager != null)
				_containerManager.Dispose();
		}

		protected virtual ComposablePartCatalog CreateBaseCatalog()
		{
			return new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
		}
	}
}

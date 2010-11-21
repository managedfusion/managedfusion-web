using System;
using System.Collections;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;

namespace ManagedFusion.Web.Composition
{
	public class WebScopedContainerManager : IDisposable
	{
		private readonly ComposablePartCatalog _rootCatalog;
		private readonly CompositionContainer _container;

		private const string MefContainerKey = "mef-container";
		private const string ModeKey = "Mode";

		#region Scoping Helper

		private static bool GetAllApplicationScopedDefinitions(ComposablePartDefinition def)
		{
			return def.ExportDefinitions.
				Any(ed =>
					!ed.Metadata.ContainsKey(ModeKey) ||
					(ed.Metadata.ContainsKey(ModeKey) && ((WebScopeMode)ed.Metadata[ModeKey]) == WebScopeMode.Application));
		}

		private static bool GetAllRequestScopedDefinitions(ComposablePartDefinition def)
		{
			return def.ExportDefinitions.
				Any(ed =>
					(ed.Metadata.ContainsKey(ModeKey) && ((WebScopeMode)ed.Metadata[ModeKey]) == WebScopeMode.Request));
		}

		#endregion

		public WebScopedContainerManager(ComposablePartCatalog rootCatalog)
		{
			if (rootCatalog == null) throw new ArgumentNullException("rootCatalog");

			_rootCatalog = rootCatalog;
			_container = new CompositionContainer(new FilteredCatalog(rootCatalog, def => GetAllApplicationScopedDefinitions(def)), true);
		}

		public CompositionContainer ApplicationContainer
		{
			get { return _container; }
		}

		public void CreateRequestContainer(IDictionary dictionary)
		{
			if (dictionary == null) 
				throw new ArgumentNullException("dictionary");

			var catalog = new FilteredCatalog(_rootCatalog, def => GetAllRequestScopedDefinitions(def));
			var requestContainer = new CompositionContainer(catalog, false, ApplicationContainer);

			dictionary[MefContainerKey] = requestContainer;
		}

		public void EndRequestContainer(IDictionary dictionary)
		{
			if (dictionary == null) 
				throw new ArgumentNullException("dictionary");

			CompositionContainer requestContainer = (CompositionContainer)dictionary[MefContainerKey];

			if (requestContainer != null)
				requestContainer.Dispose();

			dictionary.Remove(MefContainerKey);
		}

		public CompositionContainer GetRequestContainer(IDictionary dictionary)
		{
			if (dictionary == null) 
				throw new ArgumentNullException("dictionary");

			CompositionContainer requestContainer = (CompositionContainer)dictionary[MefContainerKey];

			if (requestContainer == null)
				throw new ApplicationException("No container was found for this request.");

			return requestContainer;
		}

		public void Dispose()
		{
			if (_container != null)
				_container.Dispose();

			if (_rootCatalog != null)
				_rootCatalog.Dispose();
		}
	}
}

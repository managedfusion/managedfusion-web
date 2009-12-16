using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Globalization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class DefaultAttribute : NameAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultAttribute"/> class.
		/// </summary>
		/// <param name="default">The @default.</param>
		public DefaultAttribute(object @default)
			: this ("", @default) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultAttribute"/> class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="default"></param>
		public DefaultAttribute(string name, object @default)
			: base(name)
		{
			if (@default == null)
				throw new ArgumentNullException("default");

			Default = @default;
		}

		/// <summary>
		/// Gets or sets the default.
		/// </summary>
		/// <value>The default.</value>
		public object Default
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the binder.
		/// </summary>
		/// <returns></returns>
		public override IModelBinder GetBinder()
		{
			return new DefaultValueModelBinder(Name, Default);
		}

		/// <summary>
		/// 
		/// </summary>
		private class DefaultValueModelBinder : ParameterNameModelBinder
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DefaultAttribute"/> class.
			/// </summary>
			/// <param name="default">The @default.</param>
			public DefaultValueModelBinder(string name, object @default)
				: base(name)
			{
				if (@default == null)
					throw new ArgumentNullException("default");

				Default = @default;
			}

			/// <summary>
			/// Gets or sets the default.
			/// </summary>
			/// <value>The default.</value>
			public object Default
			{
				get;
				private set;
			}

			/// <summary>
			/// Binds the model.
			/// </summary>
			/// <param name="controllerContext">The controller context.</param>
			/// <param name="bindingContext">The binding context.</param>
			/// <returns></returns>
			public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
			{
				if (bindingContext == null)
					throw new ArgumentNullException("bindingContext");

				var model = base.BindModel(controllerContext, bindingContext);

				// return the default if the model couldn't be retreived
				return model ?? Default;
			}
		}
	}
}

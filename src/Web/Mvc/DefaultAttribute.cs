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
			Triggers = new object[0];
		}

		private void MapCommonTriggers()
		{
			if (Triggers.Length > 0)
				return;

			if (Default is String)
				Triggers = new[] { "" };
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
		/// Gets or sets the empty values that trigger the default value.
		/// </summary>
		/// <remarks><see langref="null" /> is always considered a trigger.</remarks>
		public object[] Triggers
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the binder.
		/// </summary>
		/// <returns></returns>
		public override IModelBinder GetBinder()
		{
			MapCommonTriggers();
			return new DefaultValueModelBinder(Name, Default, Triggers);
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
			public DefaultValueModelBinder(string name, object @default, object[] triggers)
				: base(name)
			{
				if (@default == null)
					throw new ArgumentNullException("default");

				Default = @default;
				Triggers = new List<object>(triggers);
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
			/// Gets or sets the empty values that trigger the default value.
			/// </summary>
			/// <remarks><see langref="null" /> is always considered a trigger.</remarks>
			public IList<object> Triggers
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

				if (Triggers.Contains(model))
					return Default;

				// return the default if the model couldn't be retreived
				return model ?? Default;
			}
		}
	}
}

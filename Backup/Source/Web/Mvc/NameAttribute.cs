using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class NameAttribute : CustomModelBinderAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NameAttribute"/> class.
		/// </summary>
		/// <param name="default">The name.</param>
		public NameAttribute(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			Name = name;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
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
			return new ParameterNameModelBinder(Name);
		}

		/// <summary>
		/// 
		/// </summary>
		protected class ParameterNameModelBinder : Attribute, IModelBinder
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DefaultAttribute"/> class.
			/// </summary>
			/// <param name="default">The @default.</param>
			public ParameterNameModelBinder(string name)
			{
				if (name == null)
					throw new ArgumentNullException("name");

				Name = name;
			}

			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>The name.</value>
			public string Name
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
			public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
			{
				if (bindingContext == null)
					throw new ArgumentNullException("bindingContext");

				if (String.IsNullOrEmpty(Name))
					Name = bindingContext.ModelName;

				ValueProviderResult result = bindingContext.ValueProvider.GetValue(Name);

				if (result != null)
				{
					var rawValue = result.RawValue;
					var destinationType = bindingContext.ModelType;

					if (rawValue != null)
					{
						var value = result.ConvertTo(destinationType);

						if (value != null)
							return value;
					}
				}

				// cannot convert value, returning null
				return null;
			}
		}
	}
}

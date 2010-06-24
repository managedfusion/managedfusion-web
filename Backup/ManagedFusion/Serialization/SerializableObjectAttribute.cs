using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public class SerializableObjectAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializationObjectAttribute"/> class.
		/// </summary>
		public SerializableObjectAttribute()
			: this(null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializationObjectAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public SerializableObjectAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }
	}
}

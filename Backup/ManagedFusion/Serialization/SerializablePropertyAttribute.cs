using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SerializablePropertyAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializationPropertyAttribute"/> class.
		/// </summary>
		public SerializablePropertyAttribute() 
			: this(null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializationPropertyAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public SerializablePropertyAttribute(string name)
		{
			Name = name;
			IsAttribute = false;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is attribute.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if this instance is attribute; otherwise, <see langword="false"/>.
		/// </value>
		public bool IsAttribute { get; set; }
	}
}

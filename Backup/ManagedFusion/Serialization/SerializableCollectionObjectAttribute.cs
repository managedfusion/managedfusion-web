using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class SerializableCollectionObjectAttribute : SerializableObjectAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionItemAttribute"/> class.
		/// </summary>
		public SerializableCollectionObjectAttribute()
			: this(null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionItemAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public SerializableCollectionObjectAttribute(string name)
			: base(name) { }

		/// <summary>
		/// Gets or sets the name of the item.
		/// </summary>
		/// <value>The name of the item.</value>
		public string ItemName { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlResult : SerializedResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlResult"/> class.
		/// </summary>
		public XmlResult()
		{
			ContentType = "text/xml";
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "xml"; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			return Data.Serialize(new XmlSerializer(Data), SerializePublicMembers);
		}

		private class XmlSerializer : ManagedFusion.Serialization.XmlSerializer
		{
			private object _serializableObject;

			/// <summary>
			/// Initializes a new instance of the <see cref="XmlSerializer"/> class.
			/// </summary>
			/// <param name="obj">The obj.</param>
			public XmlSerializer(object obj)
			{
				_serializableObject = obj;
			}

			/// <summary>
			/// Gets a value indicating whether to check for object name.
			/// </summary>
			/// <value>
			/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
			/// </value>
			public override bool CheckForObjectName
			{
				get { return true; }
			}

			/// <summary>
			/// Serializes to json.
			/// </summary>
			/// <param name="serialization">The serialization.</param>
			/// <returns></returns>
			public override string Serialize(Dictionary<string, object> serialization)
			{
				Dictionary<string, object> response = SerializedResult.BuildResponse(_serializableObject, serialization);

				Dictionary<string, object> wrapper = new Dictionary<string, object>();
				wrapper.Add("response", response);

				return base.Serialize(wrapper);
			}
		}
	}
}

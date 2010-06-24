using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ManagedFusion.Serialization
{
	public class XmlSerializer : ISerializer
	{
		private XmlDocument doc;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSerializer"/> class.
		/// </summary>
		public XmlSerializer()
		{
			CheckForObjectName = true;
			MaxSerializableLevelsSupported = null;
		}

		/// <summary>
		/// Gets a value indicating whether to check for object name.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
		/// </value>
		public virtual bool CheckForObjectName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the max levels allowed to be serialized
		/// </summary>
		public virtual int? MaxSerializableLevelsSupported
		{
			get;
			set;
		}

		/// <summary>
		/// Serializes the specified serialization.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		public virtual string Serialize(IDictionary<string, object> serialization)
		{
			doc = new XmlDocument();

			BuildObject(doc, serialization);
			doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "utf-8", "yes"), doc.DocumentElement);

			StringBuilder builder = new StringBuilder();
			using (XmlWriter writer = new XmlTextWriter(new StringWriter(builder)))
			{
				doc.WriteTo(writer);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private void BuildObject(XmlNode node, IDictionary<string, object> serialization)
		{
			foreach (var entry in serialization)
			{
				string key = entry.Key.TrimStart(new char[] { Serializer.AttributeMarker, Serializer.CollectionItemMarker });

				if ((entry.Key as string).StartsWith(Serializer.AttributeMarker.ToString()))
				{
					XmlAttribute attr = doc.CreateAttribute(key);
					BuildValue(attr, entry.Value);
					node.Attributes.Append(attr);
				}

				else
				{
					XmlElement subNode = doc.CreateElement(key);
					BuildValue(subNode, entry.Value);
					node.AppendChild(subNode);
				}
			}
		}

		/// <summary>
		/// Allows the array pass through.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		private bool AllowArrayPassThrough(object obj)
		{
			if (obj is IDictionary == false)
				return false;

			IDictionary dic = obj as IDictionary;

			if (dic.Count != 1)
				return false;

			foreach (object key in dic.Keys)
				if (key is String && (key as string).StartsWith(Serializer.CollectionItemMarker.ToString()))
					return true;

			return false;
		}

		/// <summary>
		/// Builds the array.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="array">The array.</param>
		private void BuildArray(XmlNode node, IList array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (AllowArrayPassThrough(array[i]))
					BuildValue(node, array[i]);
				else
				{
					XmlElement subNode = doc.CreateElement("item");
					BuildValue(subNode, array[i]);
					node.AppendChild(subNode);
				}
			}
		}

		/// <summary>
		/// Converts to json value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private void BuildValue(XmlNode node, object value)
		{
			if (value == null)
			{
				node.AppendChild(doc.CreateTextNode(value as string));
			}
			else if (value is IList)
			{
				BuildArray(node, value as IList);
			}
			else if (value is IDictionary<string, object>)
			{
				BuildObject(node, value as IDictionary<string, object>);
			}
			else if (value is bool) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((bool)value))); }
			else if (value is byte) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((byte)value))); }
			else if (value is char) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((char)value))); }
			else if (value is DateTime) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind))); }
			else if (value is DateTimeOffset) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((DateTimeOffset)value))); }
			else if (value is decimal) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((decimal)value))); }
			else if (value is double) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((double)value))); }
			else if (value is float) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((float)value))); }
			else if (value is Guid) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((Guid)value))); }
			else if (value is int) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((int)value))); }
			else if (value is long) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((long)value))); }
			else if (value is sbyte) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((sbyte)value))); }
			else if (value is short) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((short)value))); }
			else if (value is TimeSpan) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((TimeSpan)value))); }
			else if (value is uint) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((uint)value))); }
			else if (value is ulong) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((ulong)value))); }
			else if (value is ushort) { node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((ushort)value))); }
			else
			{
				node.AppendChild(doc.CreateTextNode(Convert.ToString(value)));
			}
		}

		/// <summary>
		/// Serializes to XML.
		/// </summary>
		/// <param name="doc">The doc.</param>
		/// <param name="name">The name.</param>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private XmlNode SerializeToXml(XmlDocument doc, string name, IDictionary serialization)
		{
			XmlNode node;

			if (String.IsNullOrEmpty(name))
				node = doc;
			else
				node = doc.CreateElement(name);

			foreach (DictionaryEntry entry in serialization)
			{
				if (!(entry.Key is string))
					throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

				if (entry.Value is IDictionary)
					node.AppendChild(SerializeToXml(doc, entry.Key as string, entry.Value as IDictionary));

				else if (entry.Value is IList<object>)
				{
					XmlElement subNode = doc.CreateElement(entry.Key as string);

					foreach (object listItem in entry.Value as IList<object>)
					{
						if (listItem is IDictionary<string, object>)
							subNode.AppendChild(SerializeToXml(doc, "item", listItem as IDictionary));
						else
						{
							XmlElement subNodeItem = doc.CreateElement("item");
							XmlText subNodeValue = doc.CreateTextNode(Convert.ToString(listItem));
							subNodeItem.AppendChild(subNodeValue);
							subNode.AppendChild(subNodeItem);
						}
					}

					node.AppendChild(subNode);
				}

				else if ((entry.Key as string).StartsWith("_"))
				{
					XmlAttribute attr = doc.CreateAttribute((entry.Key as string).Substring(1));
					attr.Value = Convert.ToString(entry.Value);
					node.Attributes.Append(attr);
				}

				else
				{
					XmlElement subNode = doc.CreateElement((entry.Key as string));
					XmlText subNodeValue = doc.CreateTextNode(Convert.ToString(entry.Value));
					subNode.AppendChild(subNodeValue);
					node.AppendChild(subNode);
				}
			}

			return node;
		}
	}
}

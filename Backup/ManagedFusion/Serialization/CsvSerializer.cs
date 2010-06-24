using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ManagedFusion.Serialization
{
	public class CsvSerializer : ISerializer
	{
		private readonly char SeperatorCharacter;

		private readonly string LineTermination;

		public CsvSerializer()
			: this(',') { }

		public CsvSerializer(char seperatorCharacter)
		{
			SeperatorCharacter = seperatorCharacter;
			LineTermination = Environment.NewLine;

			CheckForObjectName = false;
			MaxSerializableLevelsSupported = 1;
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

		public string Serialize(IDictionary<string, object> serialization)
		{
			StringBuilder builder = new StringBuilder();
			BuildHeaderLine(builder, serialization);
			BuildLine(builder, serialization);
			return builder.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="serialization"></param>
		private void BuildHeaderLine(StringBuilder builder, IDictionary<string, object> serialization)
		{
			var obj = serialization.FirstOrDefault();
			var lines = obj.Value as IList<object>;

			if (lines == null)
				lines = new List<object>(new[] { serialization });

			IDictionary line = lines.FirstOrDefault() as IDictionary;
			if (line != null)
			{
				IList<string> values = new List<string>();
				foreach (DictionaryEntry entry in line)
				{
					if (!(entry.Key is string))
						throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

					values.Add((entry.Key ?? "").ToString());
				}
				builder.Append(String.Join(SeperatorCharacter.ToString(), values.ToArray()));
				builder.Append(LineTermination);
			}
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private void BuildLine(StringBuilder builder, IDictionary<string, object> serialization)
		{
			var obj = serialization.FirstOrDefault();
			var lines = obj.Value as IList<object>;

			if (lines == null)
				lines = new List<object>(new[] { serialization });

			foreach (var line in lines.Cast<IDictionary>())
			{
				if (line != null)
				{
					IList<string> values = new List<string>();
					foreach (DictionaryEntry entry in line)
					{
						if (!(entry.Key is string))
							throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

						values.Add((entry.Value ?? "").ToString());
					}
					builder.Append(String.Join(SeperatorCharacter.ToString(), values.ToArray()));
					builder.Append(LineTermination);
				}
			}
		}
	}
}

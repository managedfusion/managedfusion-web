using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// Possible JSON tokens in parsed input.
	/// </summary>
	public enum JsonToken
	{
		Unknown,
		LeftBrace,
		RightBrace,
		Colon,
		Comma,
		LeftBracket,
		RightBracket,
		String,
		Number,
		True,
		False,
		Null
	}

	/// <summary>
	/// Exception raised when <see cref="JsonParser" /> encounters an invalid token.
	/// </summary>
	public class InvalidJsonException : Exception
	{
		public InvalidJsonException(string message)
			: base(message) { }
	}

	/// <seealso cref="http://json.org" />
	/// <seealso href="http://dimebrain.com/2010/04/how-to-parse-json.html"/>
	public class JsonDeserializer
	{
		private const NumberStyles JsonNumbers = NumberStyles.Float;
		private static readonly IDictionary<Type, PropertyInfo[]> _cache;

		private static readonly char[] _base16 = new[]
                             {
                                 '0', '1', '2', '3', 
                                 '4', '5', '6', '7', 
                                 '8', '9', 'A', 'B', 
                                 'C', 'D', 'E', 'F'
                             };

		static JsonDeserializer()
		{
			_cache = new Dictionary<Type, PropertyInfo[]>(0);
		}

		public static IDictionary<string, object> Deserialize(string json)
		{
			var data = json.ToCharArray();
			var index = 0;

			return ParseObject(data, ref index);
		}

		internal static Dictionary<string, object> InitializeBag()
		{
			return new Dictionary<string, object>(
				0, StringComparer.OrdinalIgnoreCase
				);
		}

		internal static KeyValuePair<string, object> ParsePair(IList<char> data, ref int index)
		{
			var valid = true;

			var name = ParseString(data, ref index);
			if (name == null)
			{
				valid = false;
			}

			if (!ParseToken(JsonToken.Colon, data, ref index))
			{
				valid = false;
			}

			if (!valid)
			{
				throw new InvalidJsonException(string.Format(
							"Invalid JSON found while parsing a value pair at index {0}.", index
							));
			}

			index++;
			var value = ParseValue(data, ref index);
			return new KeyValuePair<string, object>(name, value);
		}

		internal static bool ParseToken(JsonToken token, IList<char> data, ref int index)
		{
			var nextToken = NextToken(data, ref index);
			return token == nextToken;
		}

		internal static string ParseString(IList<char> data, ref int index)
		{
			var symbol = data[index];
			IgnoreWhitespace(data, ref index, symbol);
			symbol = data[++index]; // Skip first quotation

			var sb = new StringBuilder();
			while (true)
			{
				if (index >= data.Count - 1)
				{
					return null;
				}
				switch (symbol)
				{
					case '"':  // End String
						index++;
						return sb.ToString();
					case '\\': // Control Character
						symbol = data[++index];
						switch (symbol)
						{
							case '\\':
							case '/':
							case 'b':
							case 'f':
							case 'n':
							case 'r':
							case 't':
								sb.Append('\\').Append(symbol);
								break;
							case 'u': // Unicode literals
								if (index < data.Count - 5)
								{
									var array = data.ToArray();
									var buffer = new char[4];
									Array.Copy(array, index + 1, buffer, 0, 4);

									// http://msdn.microsoft.com/en-us/library/aa664669%28VS.71%29.aspx
									// http://www.yoda.arachsys.com/csharp/unicode.html
									// http://en.wikipedia.org/wiki/UTF-32/UCS-4
									var hex = new string(buffer);
									var unicode = (char)Convert.ToInt32(hex, 16);
									sb.Append(unicode);
									index += 4;
								}
								else
								{
									break;
								}
								break;
						}
						break;
					default:
						sb.Append(symbol);
						break;
				}
				symbol = data[++index];
			}
		}

		internal static object ParseValue(IList<char> data, ref int index)
		{
			var token = NextToken(data, ref index);
			switch (token)
			{
				// End Tokens
				case JsonToken.RightBracket:    // Bad Data
				case JsonToken.RightBrace:
				case JsonToken.Unknown:
				case JsonToken.Colon:
				case JsonToken.Comma:
					throw new InvalidJsonException(string.Format(
							"Invalid JSON found while parsing a value at index {0}.", index
							));
				// Value Tokens
				case JsonToken.LeftBrace:
					return ParseObject(data, ref index);
				case JsonToken.LeftBracket:
					return ParseArray(data, ref index);
				case JsonToken.String:
					return ParseString(data, ref index);
				case JsonToken.Number:
					return ParseNumber(data, ref index);
				case JsonToken.True:
					return true;
				case JsonToken.False:
					return false;
				case JsonToken.Null:
					return null;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		internal static IDictionary<string, object> ParseObject(IList<char> data, ref int index)
		{
			var result = InitializeBag();

			index++; // Skip first brace
			while (index < data.Count - 1)
			{
				var token = NextToken(data, ref index);
				switch (token)
				{
					// End Tokens
					case JsonToken.Unknown:             // Bad Data
					case JsonToken.True:
					case JsonToken.False:
					case JsonToken.Null:
					case JsonToken.Comma:
					case JsonToken.Colon:
					case JsonToken.RightBracket:
					case JsonToken.Number:
						throw new InvalidJsonException(string.Format(
							"Invalid JSON found while parsing an object at index {0}.", index
							));
					case JsonToken.RightBrace:          // End Object
						return result;
					// Start Tokens
					case JsonToken.LeftBrace:           // Start Object
						var @object = ParseObject(data, ref index);
						if (@object != null)
						{
							result.Add("object", @object);
						}
						break;
					case JsonToken.LeftBracket:         // Start Array
						var @array = ParseArray(data, ref index);
						if (@array != null)
						{
							result.Add("array", @array);
						}
						break;
					case JsonToken.String:
						var pair = ParsePair(data, ref index);
						result.Add(pair.Key, pair.Value);
						break;
					default:
						throw new NotSupportedException("Invalid token expected.");
				}

				index++;
			}

			return result;
		}

		internal static IEnumerable<object> ParseArray(IList<char> data, ref int index)
		{
			var result = new List<object>();

			index++; // Skip first bracket
			while (index < data.Count - 1)
			{
				var token = NextToken(data, ref index);
				switch (token)
				{
					// End Tokens
					case JsonToken.Unknown:             // Bad Data
						throw new InvalidJsonException(string.Format(
							"Invalid JSON found while parsing an array at index {0}.", index
							));
					case JsonToken.RightBracket:        // End Array
						return result;
					// Skip Tokens
					case JsonToken.Comma:               // Separator
					case JsonToken.RightBrace:          // End Object
					case JsonToken.Colon:               // Separator
						break;
					// Value Tokens
					case JsonToken.LeftBrace:           // Start Object
					case JsonToken.LeftBracket:         // Start Array
					case JsonToken.String:
					case JsonToken.Number:
					case JsonToken.True:
					case JsonToken.False:
					case JsonToken.Null:
						var value = ParseValue(data, ref index);
						result.Add(value);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				index++;
			}

			return result;
		}

		internal static object ParseNumber(IList<char> data, ref int index)
		{
			var symbol = data[index];
			IgnoreWhitespace(data, ref index, symbol);

			var start = index;
			var length = 0;
			while (ParseToken(JsonToken.Number, data, ref index))
			{
				length++;
				index++;
			}

			var number = new char[length];
			Array.Copy(data.ToArray(), start, number, 0, length);

			double result;
			var buffer = new string(number);

			if (!double.TryParse(buffer, JsonNumbers, CultureInfo.InvariantCulture, out result))
			{
				throw new InvalidJsonException(
					string.Format("Value '{0}' was not a valid JSON number", buffer)
					);
			}

			return result;
		}

		internal static JsonToken NextToken(IList<char> data, ref int index)
		{
			var symbol = data[index];
			var token = GetTokenFromSymbol(symbol);
			token = IgnoreWhitespace(data, ref index, ref token, symbol);

			GetKeyword("true", JsonToken.True, data, ref index, ref token);
			GetKeyword("false", JsonToken.False, data, ref index, ref token);
			GetKeyword("null", JsonToken.Null, data, ref index, ref token);

			return token;
		}

		internal static JsonToken GetTokenFromSymbol(char symbol)
		{
			return GetTokenFromSymbol(symbol, JsonToken.Unknown);
		}

		internal static JsonToken GetTokenFromSymbol(char symbol, JsonToken token)
		{
			switch (symbol)
			{
				case '{':
					token = JsonToken.LeftBrace;
					break;
				case '}':
					token = JsonToken.RightBrace;
					break;
				case ':':
					token = JsonToken.Colon;
					break;
				case ',':
					token = JsonToken.Comma;
					break;
				case '[':
					token = JsonToken.LeftBracket;
					break;
				case ']':
					token = JsonToken.RightBracket;
					break;
				case '"':
					token = JsonToken.String;
					break;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '.':
				case 'e':
				case 'E':
				case '+':
				case '-':
					token = JsonToken.Number;
					break;
			}
			return token;
		}

		internal static void IgnoreWhitespace(IList<char> data, ref int index, char symbol)
		{
			var token = JsonToken.Unknown;
			IgnoreWhitespace(data, ref index, ref token, symbol);
			return;
		}

		internal static JsonToken IgnoreWhitespace(IList<char> data, ref int index, ref JsonToken token, char symbol)
		{
			switch (symbol)
			{
				case ' ':
				case '\\':
				case '/':
				case '\b':
				case '\f':
				case '\n':
				case '\r':
				case '\t':
					index++;
					token = NextToken(data, ref index);
					break;
			}
			return token;
		}

		internal static void GetKeyword(string word, JsonToken target, IList<char> data, ref int index, ref JsonToken result)
		{
			var buffer = data.Count - index;
			if (buffer < word.Length)
			{
				return;
			}

			for (var i = 0; i < word.Length; i++)
			{
				if (data[index + i] != word[i])
				{
					return;
				}
			}

			result = target;
			index += word.Length;
		}

		internal static string BaseConvert(int input, char[] charSet, int minLength)
		{
			var sb = new StringBuilder();
			var @base = charSet.Length;

			while (input > 0)
			{
				var index = input % @base;
				sb.Insert(0, new[] { charSet[index] });
				input = input / @base;
			}

			while (sb.Length < minLength)
			{
				sb.Insert(0, "0");
			}

			return sb.ToString();
		}
	}
}
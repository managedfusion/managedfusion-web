using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace System
{
	/// <summary>
	/// 
	/// </summary>
	public static class StringExtensions
	{
		private static readonly Regex UrlReplacementExpression = new Regex(@"[^0-9a-z \-]*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

		/// <summary>
		/// Tries the trim.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public static string TryTrim(this string s)
		{
			if (s == null)
				return s;

			return s.Trim();
		}

		/// <summary>
		/// Creates the hash algorithm.
		/// </summary>
		/// <param name="hashName">Name of the hash.</param>
		/// <returns></returns>
		private static HashAlgorithm CreateHashAlgorithm(string hashName)
		{
			switch (hashName.ToLower())
			{
				case "crc32":
					return new ManagedFusion.Crc32();

				default:
					return HashAlgorithm.Create(hashName);
			}
		}

		/// <summary>
		/// Toes the hash.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		public static byte[] ToHash(this string content)
		{
			return ToHash("MD5");
		}

		/// <summary>
		/// Toes the hash string.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="hashName">Name of the hash.</param>
		/// <returns></returns>
		public static byte[] ToHash(this string content, string hashName)
		{
			if (content == null)
				throw new ArgumentNullException("content");

			HashAlgorithm algorithm = CreateHashAlgorithm(hashName);
			byte[] buffer = algorithm.ComputeHash(Encoding.Default.GetBytes(content));
			return buffer;
		}

		/// <summary>
		/// Toes the hash.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		public static string ToHashString(this string content)
		{
			return ToHashString(content, "MD5");
		}

		/// <summary>
		/// Toes the hash string.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="hashName">Name of the hash.</param>
		/// <returns></returns>
		public static string ToHashString(this string content, string hashName)
		{
			byte[] buffer = ToHash(content, hashName);
			StringBuilder builder = new StringBuilder(buffer.Length * 2);

			foreach (byte b in buffer)
				builder.Append(b.ToString("x2"));

			return builder.ToString();
		}

		/// <summary>
		/// Toes the hash int64.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		public static long ToHashInt64(this string content)
		{
			return ToHashInt64(content, "MD5");
		}

		/// <summary>
		/// Toes the hash int64.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="hashName">Name of the hash.</param>
		/// <returns></returns>
		public static long ToHashInt64(this string content, string hashName)
		{
			byte[] buffer = ToHash(content, hashName);
			return BitConverter.ToInt64(buffer, 0);
		}

		/// <summary>
		/// Toes the URL part.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		public static string ToUrlFormat(this string content)
		{
			return UrlReplacementExpression.Replace(content.Trim(), String.Empty).Replace(' ', '-').ToLowerInvariant();
		}

		/// <summary>
		/// Tries to create a phrase string from Pascal case text.
		/// Will place spaces before capitalized letters.
		/// 
		/// Note that this method may not work for round tripping 
		/// ToPascalCase calls, since ToPascalCase strips more characters
		/// than just spaces.
		/// </summary>
		/// <param name="camelCase"></param>
		/// <returns></returns>
		public static string FromPascalCase(this string pascalCase)
		{
			if (pascalCase == null)
				throw new ArgumentNullException("camelCase");

			StringBuilder sb = new StringBuilder(pascalCase.Length + 10);
			bool first = true;
			char lastChar = '\0';

			foreach (char ch in pascalCase)
			{
				if (!first && (Char.IsUpper(ch) || Char.IsDigit(ch) && !Char.IsDigit(lastChar)))
					sb.Append(' ');

				// append the character to the string builder
				sb.Append(ch);

				first = false;
				lastChar = ch;
			}

			return sb.ToString();
		}

		/// <summary>
		/// Takes a phrase and turns it into Pascal case text.
		/// White Space, punctuation and separators are stripped
		/// </summary>
		/// <param name="?"></param>
		public static string ToPascalCase(this string phrase)
		{
			if (phrase == null)
				return String.Empty;

			StringBuilder sb = new StringBuilder(phrase.Length);

			// First letter is always upper case
			bool nextUpper = true;

			foreach (char ch in phrase)
			{
				if (Char.IsWhiteSpace(ch) || Char.IsPunctuation(ch) || Char.IsSeparator(ch))
				{
					nextUpper = true;
					continue;
				}

				if (nextUpper)
					sb.Append(Char.ToUpper(ch));
				else
					sb.Append(Char.ToLower(ch));

				nextUpper = false;
			}

			return sb.ToString();
		}
	}
}

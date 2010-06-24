using System;
using System.Security.Cryptography;

namespace ManagedFusion
{
	/// <summary>
	/// 
	/// </summary>
	public class Crc32 : HashAlgorithm
	{
		public const uint DefaultPolynomial = 0xedb88320;
		public const uint DefaultSeed = 0xffffffff;

		private uint hash;
		private uint seed;
		private uint[] table;

		/// <summary>
		/// Initializes a new instance of the <see cref="Crc32"/> class.
		/// </summary>
		public Crc32()
		{
			table = InitializeTable(DefaultPolynomial);
			seed = DefaultSeed;
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Crc32"/> class.
		/// </summary>
		/// <param name="polynomial">The polynomial.</param>
		/// <param name="seed">The seed.</param>
		public Crc32(uint polynomial, uint seed)
		{
			table = InitializeTable(polynomial);
			this.seed = seed;
			Initialize();
		}

		/// <summary>
		/// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm"/> class.
		/// </summary>
		public override void Initialize()
		{
			hash = seed;
		}

		/// <summary>
		/// Hashes the core.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="start">The start.</param>
		/// <param name="length">The length.</param>
		protected override void HashCore(byte[] buffer, int start, int length)
		{
			hash = CalculateHash(table, hash, buffer, start, length);
		}

		/// <summary>
		/// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
		/// </summary>
		/// <returns>The computed hash code.</returns>
		protected override byte[] HashFinal()
		{
			byte[] hashBuffer = UInt32ToBigEndianBytes(hash);
			this.HashValue = hashBuffer;
			return hashBuffer;
		}

		/// <summary>
		/// Gets the size, in bits, of the computed hash code.
		/// </summary>
		/// <value></value>
		/// <returns>The size, in bits, of the computed hash code.</returns>
		public override int HashSize
		{
			get { return 32; }
		}

		/// <summary>
		/// Computes the specified polynomial.
		/// </summary>
		/// <param name="polynomial">The polynomial.</param>
		/// <param name="seed">The seed.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns></returns>
		public static uint Compute(uint polynomial, uint seed, byte[] buffer)
		{
			return CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Initializes the table.
		/// </summary>
		/// <param name="polynomial">The polynomial.</param>
		/// <returns></returns>
		private static UInt32[] InitializeTable(uint polynomial)
		{
			uint[] createTable = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint entry = (uint)i;
				for (int j = 0; j < 8; j++)
					if ((entry & 1) == 1)
						entry = (entry >> 1) ^ polynomial;
					else
						entry = entry >> 1;
				createTable[i] = entry;
			}
			return createTable;
		}

		/// <summary>
		/// Calculates the hash.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="seed">The seed.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="start">The start.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, int start, int size)
		{
			uint crc = seed;
			for (int i = start; i < size; i++)
				unchecked
				{
					crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
				}
			return ~crc;
		}

		/// <summary>
		/// Us the int32 to big endian bytes.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		private byte[] UInt32ToBigEndianBytes(uint x)
		{
			return new byte[] {
				(byte)((x >> 24) & 0xff),
				(byte)((x >> 16) & 0xff),
				(byte)((x >> 8) & 0xff),
				(byte)(x & 0xff)
			};
		}
	}
}
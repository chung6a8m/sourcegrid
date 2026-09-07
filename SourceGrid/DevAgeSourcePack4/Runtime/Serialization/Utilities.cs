/*
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
*/
using System;
using System.IO;
using System.Text.Json;
namespace DevAge.Runtime.Serialization
{
	/*
	/// <summary>
	/// Static Class for serialization utilities
	/// </summary>
	public static class Utilities
	{
		#region Serialization Code
        /// <summary>
        /// Deserialize the stream. Using BinaryFormatter.
        /// </summary>
        /// <param name="p_Stream"></param>
        /// <returns></returns>
		public static object BinDeserialize(Stream p_Stream)
		{
			BinaryFormatter f = new BinaryFormatter();
			object tmp;
			tmp = f.Deserialize(p_Stream);
			return tmp;
		}

        /// <summary>
        /// Serialize the stream. Using BinaryFormatter.
        /// </summary>
        /// <param name="p_Stream"></param>
        /// <param name="p_Object"></param>
		public static void BinSerialize(Stream p_Stream, object p_Object)
		{
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(p_Stream,p_Object);
		}

        /// <summary>
        /// Deserialize the specified file. Using BinaryFormatter.
        /// </summary>
        /// <param name="p_strFileName"></param>
        /// <returns></returns>
		public static object BinDeserialize(string p_strFileName)
		{
			object tmp;
			using (FileStream l_Stream = new FileStream(p_strFileName,FileMode.Open,FileAccess.Read))
			{
				tmp = BinDeserialize(l_Stream);
				l_Stream.Close();
			}
			return tmp;
		}

        /// <summary>
        /// Serialize the object to the specified file. Using BinaryFormatter.
        /// </summary>
        /// <param name="p_strFileName"></param>
        /// <param name="p_Object"></param>
		public static void BinSerialize(string p_strFileName, object p_Object)
		{
			using (FileStream l_Stream = new FileStream(p_strFileName,FileMode.Create,FileAccess.Write))
			{
				BinSerialize(l_Stream,p_Object);
				l_Stream.Close();
			}
		}

		#endregion
	}*/

	/// <summary>
	/// Static Class for serialization utilities
	/// </summary>
	public static class Utilities
	{
		#region Serialization Code
		/// <summary>
		/// Deserialize the stream. Using System.Text.Json.
		/// </summary>
		/// <param name="p_Stream"></param>
		/// <param name="p_Type">The target type to deserialize into (Optional but highly recommended for System.Text.Json).</param>
		/// <returns></returns>
		public static object BinDeserialize(Stream p_Stream, Type p_Type = null)
		{
			// Nếu không truyền Type, nó sẽ trả về JsonElement (mặc định của System.Text.Json cho kiểu object)
			if (p_Type == null)
			{
				return JsonSerializer.Deserialize<object>(p_Stream);
			}

			return JsonSerializer.Deserialize(p_Stream, p_Type);
		}

		/// <summary>
		/// Serialize the stream. Using System.Text.Json.
		/// </summary>
		/// <param name="p_Stream"></param>
		/// <param name="p_Object"></param>
		public static void BinSerialize(Stream p_Stream, object p_Object)
		{
			JsonSerializer.Serialize(p_Stream, p_Object);
		}

		/// <summary>
		/// Deserialize the specified file. Using System.Text.Json.
		/// </summary>
		/// <param name="p_strFileName"></param>
		/// <param name="p_Type">The target type to deserialize into (Optional but highly recommended for System.Text.Json).</param>
		/// <returns></returns>
		public static object BinDeserialize(string p_strFileName, Type p_Type = null)
		{
			object tmp;
			using (FileStream l_Stream = new FileStream(p_strFileName, FileMode.Open, FileAccess.Read))
			{
				tmp = BinDeserialize(l_Stream, p_Type);
			}
			return tmp;
		}

		/// <summary>
		/// Serialize the object to the specified file. Using System.Text.Json.
		/// </summary>
		/// <param name="p_strFileName"></param>
		/// <param name="p_Object"></param>
		public static void BinSerialize(string p_strFileName, object p_Object)
		{
			using (FileStream l_Stream = new FileStream(p_strFileName, FileMode.Create, FileAccess.Write))
			{
				BinSerialize(l_Stream, p_Object);
			}
		}
		#endregion
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ista.Utilities
{
    public static class ByteArrayConverters
    {
        /// <summary>
        /// Decimals to byte array.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <returns>Byte array of the decimal value.</returns>
        public static byte[] DecimalToByteArray(decimal src)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(src);
                    return stream.ToArray();
                }
            }
        }
        /// <summary>
        /// Bytes the array to decimal.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <returns>Decimal value </returns>
        public static decimal ByteArrayToDecimal(byte[] src)
        {
            using (MemoryStream stream = new MemoryStream(src))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return reader.ReadDecimal();
                }
            }
        }
        /// <summary>
        /// STRs to byte array.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>

        public static byte[] StrToByteArray(string str)
        {
            return StrToByteArray(str, null);
        }

        public static byte[] StrToByteArray(string str, System.Text.Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = new System.Text.UTF8Encoding();
            }
            byte[] encoded = encoding.GetBytes(str);

            byte[] returnedBytes = new byte[16];

            encoded.CopyTo(returnedBytes,0);

            return returnedBytes;
        }
        /// <summary>
        /// Convert Bytes array to string.
        /// </summary>
        /// <param name="Bytes">The bytes.</param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] Bytes)
        {
            return ByteArrayToString(Bytes, null);
        }
        /// <summary>
        /// Convert Bytes array to string.
        /// </summary>
        /// <param name="Bytes">bytes</param>
        /// <param name="encoding">Encoding type</param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] Bytes, System.Text.Encoding encoding)
        {
            string str = null;
            if (encoding == null)
            {
                encoding = new System.Text.UTF8Encoding();
            }
            int a = 0;
            foreach (byte _byte in Bytes)
            {
                if (_byte < 48)
                {
                    Bytes[a] += 48;
                }
                a++;
            }
            str = encoding.GetString(Bytes);
            return str;
        }
    }
}

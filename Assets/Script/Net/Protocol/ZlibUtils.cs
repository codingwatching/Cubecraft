﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zlib;

namespace Cubecraft.Net.Protocol
{
    /// <summary>
    /// Quick Zlib compression handling for network packet compression.
    /// Note: Underlying compression handling is taken from the DotNetZip Library.
    /// This library is open source and provided under the Microsoft Public License.
    /// More info about DotNetZip at dotnetzip.codeplex.com.
    /// </summary>
    public static class ZlibUtils
    {
        /// <summary>
        /// Compress a byte array into another bytes array using Zlib compression
        /// </summary>
        /// <param name="to_compress">Data to compress</param>
        /// <returns>Compressed data as a byte array</returns>
        public static byte[] Compress(byte[] to_compress, MemoryStream memstream)
        {
            byte[] data;
            using (ZlibStream stream = new ZlibStream(memstream, CompressionMode.Compress))
            {
                stream.Write(to_compress, 0, to_compress.Length);
            }
            data = memstream.ToArray();
            return data;
        }

        /// <summary>
        /// Decompress a byte array into another byte array of the specified size
        /// </summary>
        /// <param name="to_decompress">Data to decompress</param>
        /// <param name="size_uncompressed">Size of the data once decompressed</param>
        /// <returns>Decompressed data as a byte array</returns>
        public static Stream Decompress(MemoryStream to_decompress, int size_uncompressed)
        {
            ZlibStream stream = new ZlibStream(to_decompress, CompressionMode.Decompress);
            return stream;
        }

        /// <summary>
        /// Decompress a byte array into another byte array of a potentially unlimited size (!)
        /// </summary>
        /// <param name="to_decompress">Data to decompress</param>
        /// <returns>Decompressed data as byte array</returns>
        public static byte[] Decompress(byte[] to_decompress)
        {
            ZlibStream stream = new ZlibStream(new System.IO.MemoryStream(to_decompress, false), CompressionMode.Decompress);
            byte[] buffer = new byte[16 * 1024];
            using (System.IO.MemoryStream decompressedBuffer = new System.IO.MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    decompressedBuffer.Write(buffer, 0, read);
                return decompressedBuffer.ToArray();
            }
        }
    }
}

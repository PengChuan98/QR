using QuickRemember.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Others
{
    public static class MetaWordsManager
    {
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        /// <param name="serialize"></param>
        /// <returns></returns>
        public static bool Serialize(List<MetaWord>? words, out byte[]? serialize)
        {
            serialize = null;
            if (words == null || words.Count == 0)
            {
                return false;
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(words);
            serialize = Compress(System.Text.Encoding.UTF8.GetBytes(json));
            return true;
        }

        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(zippedData);
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
            System.IO.MemoryStream outBuffer = new System.IO.MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialize"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool Deserialize(byte[] serialize, out List<MetaWord>? words)
        {
            words = null;
            if (serialize == null || serialize.Length == 0)
            {
                return false;
            }
            var json = System.Text.Encoding.UTF8.GetString(Decompress(serialize));
            words = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MetaWord>>(json);
            return true;
        }
    }
}

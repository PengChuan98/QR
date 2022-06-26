using QuickRemember.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Helper
{
    public static class MetaWordHelper
    {
        /// <summary>
        /// 通过CSV文件初始化MetaWord数据集
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool LoadFromCSV(ref List<MetaWord> collection, string path)
        {
            if (collection == null) collection = new();
            collection.Clear();

            try
            {
                var content = CSVHelper.LoadCSVFile(path);
                var parse = CSVHelper.Parse(content, 2);
                foreach (var item in parse)
                {
                    collection.Add(new MetaWord(item[0], item[1]));
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool LoadFromBytes(ref List<MetaWord> collection, string path)
        {
            if (collection == null) collection = new();
            collection.Clear();

            try
            {
                var steam = System.IO.File.ReadAllBytes(path);
                if (!Others.MetaWordsManager.Deserialize(steam, out collection))
                {
                    throw new Exception("Deserialize 错误");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool SaveAsBytes(List<MetaWord>? collection, string path)
        {
            if (collection == null) return false;
            byte[]? stream;

            try
            {
                if (Others.MetaWordsManager.Serialize(collection, out stream) && stream != null)
                {
                    System.IO.File.WriteAllBytes(path, stream);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

            return true;
        }
    }
}

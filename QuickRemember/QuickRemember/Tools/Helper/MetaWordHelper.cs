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
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool LoadFromCSV(ref List<MetaWord> metaCollection,string path)
        {
            try
            {
                var content = CSVHelper.LoadCSVFile(path);
                var parse = CSVHelper.Parse(content, 2);
                foreach (var item in parse)
                {
                    metaCollection.Add(new MetaWord(item[0], item[1]));
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}

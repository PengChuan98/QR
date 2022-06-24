using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Helper
{
    public static class CSVHelper
    {
        /// <summary>
        /// 加载本地csv文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string LoadCSVFile(string path)
        {
            if (!System.IO.File.Exists(path)) throw new Exception("文件不存在: " + path);

            try
            {
                string file = System.IO.File.ReadAllText(path);
                return file;
            }
            catch (Exception e)
            {
                throw new Exception("文件打开失败: " + path, e);
            }
        }

        /// <summary>
        /// 解析csv字符串
        /// </summary>
        /// <param name="stream">csv数据</param>
        /// <param name="column">分割数</param>
        /// <returns>
        /// 行分割结果集合
        /// </returns>
        public static List<string[]> Parse(string stream, int? column = null)
        {
            char[] SplitArray = { ',' };
            List<string[]> data = new();

            string[] lines = stream.Split(Environment.NewLine.ToArray());

            int lineCount = lines.Length;

            if (column == null || column < 1)
            {
                throw new Exception("column is null or column less than 1.");
            }
            else
            {
                for (int i = 0; i < lineCount; i++)
                {
                    string line = lines[i].Trim();
                    if (line == "") continue;
                    string[] cells = line.Split(SplitArray, (int)column);
                    data.Add(cells);
                }
            }

            return data;
        }
    }
}

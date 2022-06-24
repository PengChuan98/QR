using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Helper
{
    public static class ListHelper<T> where T : class
    {
        /// <summary>
        /// 切割集合数据
        /// </summary>
        /// <param name="source">切割对象</param>
        /// <param name="count">每次切割个数</param>
        /// <param name="group">切割组数</param>
        /// <returns></returns>
        public static List<T> SplitList(List<T> source, int count, int group = 1)
        {
            int maxGroup = (int)Math.Ceiling((double)source.Count / (double)count);

            if (group < 0)
            {
                throw new Exception("Group must greater than 0");
            }
            else if (group == 0)
            {
                return source;
            }
            else if (group > maxGroup)
            {
                throw new Exception("Group must less than " + maxGroup.ToString());
            }
            else
            {
                List<T> result = new();

                int totalCount = source.Count;

                int actualCount = group * count < totalCount ? count : totalCount - (group - 1) * count;

                for (int i = 0; i < actualCount; i++)
                {
                    int index = (group - 1) * count + i;
                    result.Add(source[index]);
                }

                return result;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Models.Constant
{
    /// <summary>
    /// 背单词呈现方式
    /// </summary>
    public enum Pattern
    {
        /// <summary>
        /// 都不显示，听写模式
        /// </summary>
        None = 0,

        /// <summary>
        /// 同时显示
        /// 使用这个模式要用VicePanel控制中文的字体大小
        /// </summary>
        Bilingual = 1,

        /// <summary>
        /// 看单词回忆释义
        /// </summary>
        English = 2,

        /// <summary>
        /// 看释义回忆单词
        /// </summary>
        Chinese = 4,
    }
}

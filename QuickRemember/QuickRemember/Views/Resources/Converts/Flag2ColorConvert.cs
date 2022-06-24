using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Views.Resources.Converts
{
    /// <summary>
    /// 将单词的前景色
    /// </summary>
    public class Flag2ColorConvert : BaseConvert<Flag2ColorConvert>
    {
        // TODO 这里还没有实现
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.Media.Brushes.Black;
        }
    }
}

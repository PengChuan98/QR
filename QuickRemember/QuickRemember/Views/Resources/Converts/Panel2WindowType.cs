using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuickRemember.Views.Resources.Converts
{
    public class Panel2WindowType : BaseConvert<Panel2WindowType>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Binding.DoNothing;

            var pattern = (Models.Constant.PanelType)value;

            switch (pattern)
            {
                case Models.Constant.PanelType.Grid:
                    return typeof(Views.Settings.GridPanelSettingWindow);
                case Models.Constant.PanelType.List:
                    break;
                case Models.Constant.PanelType.Box:
                    break;
                default:
                    break;
            }

            return Binding.DoNothing;
        }
    }
}

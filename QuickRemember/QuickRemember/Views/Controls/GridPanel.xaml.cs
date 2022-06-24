using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickRemember.Views.Controls
{
    /// <summary>
    /// GridPanel.xaml 的交互逻辑
    /// </summary>
    public partial class GridPanel : UserControl
    {
        public double CellFontSize
        {
            get { return (double)GetValue(CellFontSizeProperty); }
            set { SetValue(CellFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellFontSizeProperty =
            DependencyProperty.Register("CellFontSize", typeof(double), typeof(GridPanel), new PropertyMetadata((double)8));



        public Stretch CellStretch
        {
            get { return (Stretch)GetValue(CellStretchProperty); }
            set { SetValue(CellStretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellStretchProperty =
            DependencyProperty.Register("CellStretch", typeof(Stretch), typeof(GridPanel), new PropertyMetadata(Stretch.None));




        public GridPanel()
        {
            InitializeComponent();
            this.DataContextChanged += GridPanel_DataContextChanged;
            this.SetBinding(CellFontSizeProperty, new Binding("FontSize") {Mode = BindingMode.TwoWay });
            this.SetBinding(CellStretchProperty, new Binding("Stretch") {Mode = BindingMode.TwoWay });

        }

        private void GridPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }
    }
}

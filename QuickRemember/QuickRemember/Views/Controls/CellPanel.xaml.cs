using QuickRemember.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// CellPanel.xaml 的交互逻辑
    /// </summary>
    public partial class CellPanel : UserControl
    {
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CellPanel), new PropertyMetadata(Stretch.None));


        private CellPanelViewModel ViewModel { get => (CellPanelViewModel)this.DataContext; }

        public CellPanel()
        {
            InitializeComponent();
            this.DataContextChanged += (s,e)=> ViewModel.Refresh();
        }

        public CellPanel(CellPanelViewModel vm)
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) => ViewModel.Refresh();

            this.DataContext = vm;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
         => ViewModel?.LeftButtonClick();

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
            => ViewModel?.RightButtonClick();

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                ViewModel?.MiddleButtonClick();
            }
        }
    }
}

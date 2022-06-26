using HandyControl.Data;
using QuickRemember.Tools.Helper;
using QuickRemember.ViewModels.Controls;
using QuickRemember.Views.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuickRemember
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void LoadWindowStyle()
        {
            var left = Properties.WindowInfos.Default.left;
            var top = Properties.WindowInfos.Default.top;
            var height = Properties.WindowInfos.Default.height;
            var width = Properties.WindowInfos.Default.width;

            if (left == 0 || top == 0 || height == 0 || width == 0)
            {
                return;
            }

            this.Left = left;
            this.Top = top;
            this.Height = height;
            this.Width = width;
        }

        private void SaveWindowStyle()
        {
            Properties.WindowInfos.Default.left = (int)this.Left;
            Properties.WindowInfos.Default.top = (int)this.Top;
            Properties.WindowInfos.Default.height = (int)this.Height;
            Properties.WindowInfos.Default.width = (int)this.Width;
            Properties.WindowInfos.Default.Save();
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadWindowStyle();
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            ((ViewModels.MVViewModel)this.DataContext).SaveDefaultSetting();
            this.SaveWindowStyle();
        }
    }
}

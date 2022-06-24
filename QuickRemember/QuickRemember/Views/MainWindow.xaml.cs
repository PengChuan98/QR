using HandyControl.Data;
using QuickRemember.Tools.Helper;
using QuickRemember.ViewModels.Controls;
using QuickRemember.Views.Settings;
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

namespace QuickRemember
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GridPanelViewModel vm;
        List<CellPanelViewModel> cellCollection = new();

        public MainWindow()
        {
            InitializeComponent();


            //string path = "";
            //if (DialogHelper.OpenFileDialog(ref path))
            //{
            //    List<Models.Core.MetaWord> metas = new();


            //    CSVHelper.Parse(CSVHelper.LoadCSVFile(path), 2).ForEach(line =>
            //    {
            //        metas.Add(new(line[0], line[1]));
            //    });



            //    vm = new(metas);
            //    this.DataContext = vm;
            //}
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = (ViewModels.Controls.GridPanelViewModel)this.DataContext;
            GridPanelSettingWindow w = new(data);
            w.Show();
        }

        internal void UpdateSkin(SkinType skin)
        {
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skin.ToString()}.xaml")
            });
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
        }

        SkinType skin = SkinType.Default;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (skin)
            {
                case SkinType.Default:
                    UpdateSkin(SkinType.Dark);
                    skin = SkinType.Dark;
                    break;
                case SkinType.Dark:
                    UpdateSkin(SkinType.Violet);
                    skin = SkinType.Violet;
                    break;
                case SkinType.Violet:
                    UpdateSkin(SkinType.Default);
                    skin = SkinType.Default;
                    break;
                default:
                    break;
            }
        }
    }
}

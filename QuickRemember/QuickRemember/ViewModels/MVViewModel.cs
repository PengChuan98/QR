using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QuickRemember.Models.Core;

namespace QuickRemember.ViewModels
{
    public class MVViewModel : ObservableObject
    {
        #region GridPanel
        private Controls.GridPanelViewModel? _gridvm;

        /// <summary>
        /// 
        /// </summary>
        public Controls.GridPanelViewModel? GPViewModel
        {
            get => _gridvm;
            set => SetProperty(ref _gridvm, value);
        }
        #endregion

        #region common
        private void InitializeCommon()
        {
            OpenSettingWindowCommand = new(this.OpenSettingWindow);

            LoadMWCAsCSVCommand = new(this.LoadMWCAsCSV);

            LoadMWCAsBytesCommand = new(this.LoadMWCAsBytes);
            SaveMWCAsBytesCommand = new(this.SaveMWCAsBytes);

            LoadRecentlyFileCommand = new(this.LoadRecentlyFile);
        }

        private void RefershRender()
        {
            // 设置界面更新
            if (SettingWindow!=null)
            {
                SettingWindow.DataContext = this.GPViewModel;
            }
        }

        private void RefreshViewModel()
        {
            switch (this.Panel)
            {
                case Models.Constant.PanelType.Grid:
                    GPViewModel = new(this.MetaWordCollection);
                    break;
                case Models.Constant.PanelType.List:
                    break;
                case Models.Constant.PanelType.Box:
                    break;
                default:
                    break;
            }
        }

        private Window? SettingWindow;

        public RelayCommand<Type> OpenSettingWindowCommand { get; set; }

        private void OpenSettingWindow(Type type)
        {
            foreach (var item in Application.Current.Windows)
            {
                if (item.GetType() == type)
                {
                    SettingWindow = (Window)item;
                    SettingWindow.Activate();

                    return;
                }
            }

            SettingWindow = (Window?)Activator.CreateInstance(type, new object?[] { GPViewModel });
            SettingWindow?.Show();

        }
        public RelayCommand? LoadMWCAsCSVCommand { get; set; }
        
        public RelayCommand LoadMWCAsBytesCommand { get; set; }

        public RelayCommand SaveMWCAsBytesCommand { get; set; }
        
        private void OpenCSVFile(string path)
        {
            Tools.Helper.MetaWordHelper.LoadFromCSV(ref this.MetaWordCollection, path);

            RefreshViewModel();
            RefershRender();

            LastOpenPath = path;
        }

        private void OpenWordsFile(string path)
        {
            Tools.Helper.MetaWordHelper.LoadFromBytes(ref this.MetaWordCollection, path);
            RefreshViewModel();
            RefershRender();

            LastOpenPath = path;
        }

        private void LoadMWCAsCSV()
        {
            if (Tools.Helper.DialogHelper.OpenFileDialog(ref Path))
            {
                OpenCSVFile(Path);
            }
        }

        private void SaveMWCAsBytes()
        {
            if (Tools.Helper.DialogHelper.SaveFileDialog(ref Path, 
                "Words Files (*.words)|*.words|(*.*)|*.*"))
            {
                Tools.Helper.MetaWordHelper.SaveAsBytes(this.MetaWordCollection,Path);

                MessageBox.Show("存储成功");
            }
        }
        
        private void LoadMWCAsBytes()
        {
            if (Tools.Helper.DialogHelper.OpenFileDialog(ref Path, 
                "Words Files (*.words)|*.words|(*.*)|*.*"))
            {
                OpenWordsFile(Path);
            }
        }

        public RelayCommand LoadRecentlyFileCommand { get; set; }

        private void LoadVMSetting()
        {
            this.Panel = (Models.Constant.PanelType)Enum.ToObject(typeof(Models.Constant.PanelType), Properties.Settings.Default.PanelType);
            switch (this.Panel)
            {
                case Models.Constant.PanelType.Grid:
                    if (GPViewModel != null)
                    {
                        GPViewModel.Rows = Properties.Settings.Default.Rows;
                        GPViewModel.Columns = Properties.Settings.Default.Columns;
                        GPViewModel.Group = Properties.Settings.Default.Group;
                        GPViewModel.FontSize = Properties.Settings.Default.FontSize;
                        GPViewModel.Stretch = (System.Windows.Media.Stretch)Enum.ToObject(typeof(System.Windows.Media.Stretch), Properties.Settings.Default.Stretch);
                        GPViewModel.Pattern = (Models.Constant.Pattern)Enum.ToObject(typeof(Models.Constant.Pattern), Properties.Settings.Default.Pattern);
                        GPViewModel.Voice = (Models.Constant.Voice)Enum.ToObject(typeof(Models.Constant.Voice), Properties.Settings.Default.Voice);
                        GPViewModel.Source = (Models.Constant.Source)Enum.ToObject(typeof(Models.Constant.Source), Properties.Settings.Default.Source);
                    }
                    break;
                case Models.Constant.PanelType.List:
                    break;
                case Models.Constant.PanelType.Box:
                    break;
                default:
                    break;
            }
        }

        private void LoadRecentlyFile()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastPath.Trim()) && System.IO.File.Exists(Properties.Settings.Default.LastPath))
            {
                string extension = System.IO.Path.GetExtension(Properties.Settings.Default.LastPath);

                if (extension.Equals(".csv"))
                {
                    this.OpenCSVFile(Properties.Settings.Default.LastPath);
                    LoadVMSetting();
                }
                else if (extension.Equals(".words"))
                {
                    this.OpenWordsFile(Properties.Settings.Default.LastPath);
                    LoadVMSetting();
                }
                else
                {
                    MessageBox.Show("无法识别文件类型");
                }
            }
        }
        #endregion

        #region command fields
        private List<MetaWord> MetaWordCollection = new();

        private string Path = "";

        private string LastOpenPath = "";
        #endregion

        #region common property
        private Models.Constant.PanelType _Panel = Models.Constant.PanelType.Grid;

        /// <summary>
        /// 
        /// </summary>
        public Models.Constant.PanelType Panel
        {
            get => _Panel;
            set => SetProperty(ref _Panel, value);
        }

        #endregion

        #region func

        private void SaveGridSetting()
        {
            Properties.Settings.Default.PanelType = (int)this.Panel;
            Properties.Settings.Default.LastPath = LastOpenPath;
            if (GPViewModel != null)
            {
                Properties.Settings.Default.Rows = GPViewModel.Rows;
                Properties.Settings.Default.Columns = GPViewModel.Columns;
                Properties.Settings.Default.Group = GPViewModel.Group;
                Properties.Settings.Default.FontSize = (int)GPViewModel.FontSize;
                Properties.Settings.Default.Stretch = (int)GPViewModel.Stretch;
                Properties.Settings.Default.Pattern = (int)GPViewModel.Pattern;
                Properties.Settings.Default.Voice = (int)GPViewModel.Voice;
                Properties.Settings.Default.Source = (int)GPViewModel.Source;
            }

            Properties.Settings.Default.Save();
              
        }

        public void SaveDefaultSetting()
        {
            switch (this.Panel)
            {
                case Models.Constant.PanelType.Grid:
                    SaveGridSetting();
                    break;
                case Models.Constant.PanelType.List:
                    break;
                case Models.Constant.PanelType.Box:
                    break;
                default:
                    break;
            }
        }
        
        #endregion

        public MVViewModel()
        {
            this.InitializeCommon();
        }
    }
}

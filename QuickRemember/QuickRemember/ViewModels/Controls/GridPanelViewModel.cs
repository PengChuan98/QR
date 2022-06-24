using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QuickRemember.Models.Core;
using QuickRemember.Tools.Helper;

namespace QuickRemember.ViewModels.Controls
{
    public class GridPanelViewModel : ObservableObject
    {
        public List<MetaWord> MetaWordCollection { get; set; } = new();
        public List<MetaWord> ShowMetaWordCollection
        {
            get => ListHelper<MetaWord>.SplitList(this.MetaWordCollection,this.Rows*this.Columns,this.Group);
        }

        public List<CellPanelViewModel> CellVMCollection { get; set; }

        private int GetMaxGroup
        {
            get
            {
                int m = (int)Math.Ceiling((double)CellVMCollection.Count / (double)(Rows * Columns));
                if (m <= 0)
                {
                    return 1;
                }
                return m;
            }
        }

        public GridPanelViewModel(List<MetaWord> metas)
        {
            this.MetaWordCollection = metas;
            CellVMCollection = new();
            MetaWordCollection.ForEach(item => { CellVMCollection.Add(new(item)); });

            this.OnCollectionChanged();
            MaxGroup = GetMaxGroup;

            this.DownLoadWebMetaWordCommand = new(this.DownLoadWebMetaWord);
        }

        #region Core

        public void OnPanelValueChanged()
        {
            if (Group > GetMaxGroup)
            {
                Group = GetMaxGroup;
            }
            MaxGroup = GetMaxGroup;
            this.OnCollectionChanged();
        }

        public void OnCollectionChanged()
        {
            this.CellPanelCollection = ListHelper<CellPanelViewModel>.SplitList(CellVMCollection, this.Rows * Columns, this.Group);
            this.OnModelValueChanged();

        }
        public void OnModelValueChanged()
            => CellPanelCollection.ForEach
            (
                item =>
                {
                    item.Pattern = this.Pattern;
                    item.Voice = this.Voice;
                    item.Source = this.Source;
                    item.Refresh();
                });
        #endregion

        #region 界面属性
        private int _rows = 10;

        /// <summary>
        /// 
        /// </summary>
        public int Rows
        {
            get => _rows;
            set
            {
                SetProperty(ref _rows, value);
                OnPanelValueChanged();
            }
        }


        private int _columns = 3;

        /// <summary>
        /// 
        /// </summary>
        public int Columns
        {
            get => _columns;
            set
            {
                SetProperty(ref _columns, value);
                OnPanelValueChanged();
            }
        }

        private int _group = 1;

        /// <summary>
        /// 
        /// </summary>
        public int Group
        {
            get => _group;
            set
            {
                SetProperty(ref _group, value);
                OnCollectionChanged();
            }
        }

        private int _maxgroup;

        /// <summary>
        /// 
        /// </summary>
        public int MaxGroup
        {
            get => _maxgroup;
            set => SetProperty(ref _maxgroup, value);
        }


        private List<CellPanelViewModel> _cellPanelCollection = new();

        /// <summary>
        /// 
        /// </summary>
        public List<CellPanelViewModel> CellPanelCollection
        {
            get => _cellPanelCollection;
            set => SetProperty(ref _cellPanelCollection, value);
        }

        #endregion

        #region 文字属性
        private double _fontsize = 20;

        /// <summary>
        /// 
        /// </summary>
        public double FontSize
        {
            get => _fontsize;
            set => SetProperty(ref _fontsize, value);
        }


        private System.Windows.Media.Stretch _stretch = System.Windows.Media.Stretch.Fill;

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Media.Stretch Stretch
        {
            get => _stretch;
            set => SetProperty(ref _stretch, value);
        }
        #endregion

        #region 模型属性
        private Models.Constant.Pattern _pattern = Models.Constant.Pattern.English;

        /// <summary>
        /// 
        /// </summary>
        public Models.Constant.Pattern Pattern
        {
            get => _pattern;
            set
            {
                SetProperty(ref _pattern, value);
                OnModelValueChanged();
            }
        }

        private Models.Constant.Voice _voice = Models.Constant.Voice.Local;

        /// <summary>
        /// 
        /// </summary>
        public Models.Constant.Voice Voice
        {
            get => _voice;
            set
            {
                SetProperty(ref _voice, value);
                CellPanelCollection.ForEach(item => item.Voice = this.Voice);
            }
        }

        private Models.Constant.Source _source = Models.Constant.Source.Native;

        /// <summary>
        /// 
        /// </summary>
        public Models.Constant.Source Source
        {
            get => _source;
            set
            {
                SetProperty(ref _source, value);
                OnModelValueChanged();
            }
        }
        #endregion

        #region 下载网络单词数据
        static DispatcherTimer?timer = null;

        Tools.Translator.BingTranslator translator;

        private void RefreshCompletedCount(object? sender, EventArgs e)
        {
            CompletedCount = (int)(100 * translator.Percentage);
            if (CompletedCount == 100)
            {
                timer?.Stop();
                CompletedCount = 100;
            }
        }

        private int _completedCount;

        /// <summary>
        /// 下载web过程中成功百分比
        /// </summary>
        public int CompletedCount
        {
            get => _completedCount;
            set => SetProperty(ref _completedCount, value);
        }

        private bool _isDownload=false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDownLoad
        {
            get => _isDownload;
            set => SetProperty(ref _isDownload, value);
        }


        public RelayCommand DownLoadWebMetaWordCommand { get; set; }

        private async void DownLoadWebMetaWord()
        {
            CompletedCount = 0;
            IsDownLoad = true;

            // 初始化
            if (timer == null)
            {
                timer = new DispatcherTimer(DispatcherPriority.Send)
                {
                    Interval = TimeSpan.FromMilliseconds(10),
                };
                timer.Tick += this.RefreshCompletedCount;
            }

            // 必须在await前面，不然就不起作用了
            timer.Start();

            translator = new(this.ShowMetaWordCollection);
            await translator.DownloadAsync();
        }


        #endregion
    }
}

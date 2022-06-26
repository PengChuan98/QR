using Microsoft.Toolkit.Mvvm.ComponentModel;
using QuickRemember.Models.Core;
using QuickRemember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickRemember.Tools;

namespace QuickRemember.ViewModels.Core
{
    public abstract class MetaWordViewModel : ObservableObject
    {

        #region 核心抽象方法
        public abstract void Refresh();

        #endregion

        #region 对外属性

        private string _show = "";

        /// <summary>
        /// 当前界面渲染的内容
        /// </summary>
        public string Show
        {
            get => _show;
            set => SetProperty(ref _show, value);
        }

        private int _flag;

        /// <summary>
        /// 当前界面的前景色
        /// </summary>
        public int Flag
        {
            get => _flag;
            set => SetProperty(ref _flag, value);
        }

        #endregion

        #region 通用字段

        /// <summary>
        /// 数据对象
        /// </summary>
        protected MetaWord Meta { get; set; } = new();

        /// <summary>
        /// 单词
        /// </summary>
        protected string Word { get => Meta.Word; }

        /// <summary>
        /// 声音源
        /// </summary>
        public Models.Constant.Voice Voice { get; set; } = Models.Constant.Voice.Local;

        /// <summary>
        /// 释义源
        /// </summary>
        public Models.Constant.Source Source { get; set; } = Models.Constant.Source.Native;

        /// <summary>
        /// 背单词模式
        /// </summary>
        public Models.Constant.Pattern Pattern { get; set; } = Models.Constant.Pattern.Bilingual;

        /// <summary>
        /// 本地释义
        /// </summary>
        protected string NativeInterpretion { get => Meta.Interpretion; }

        /// <summary>
        /// 网络释义
        /// </summary>
        protected string WebInterpretion { get => Meta.IsTrans ? Meta.WebInterpretion : Meta.Interpretion; }

        /// <summary>
        /// 能获取的有效释义
        /// </summary>
        protected string Interpretioin { get => this.Source.Equals(Models.Constant.Source.Native) ? NativeInterpretion : WebInterpretion; }

        /// <summary>
        /// 
        /// </summary>
        protected string MetaInfo { get => string.Join(" ", this.Word, this.Interpretioin); }

        #endregion

        public MetaWordViewModel(MetaWord meta) => this.Meta = meta;

        #region Core Command

        /// <summary>
        /// 显示正面信息
        /// </summary>
        public virtual void ShowFront()
        {
            switch (this.Pattern)
            {
                case Models.Constant.Pattern.None:
                    this.Show = "";
                    break;
                case Models.Constant.Pattern.Bilingual:
                    this.Show = this.MetaInfo;
                    break;
                case Models.Constant.Pattern.English:
                    this.Show = this.Word;
                    break;
                case Models.Constant.Pattern.Chinese:
                    this.Show = this.Interpretioin;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示反面信息
        /// </summary>
        public virtual void ShowBack()
        {
            switch (this.Pattern)
            {
                case Models.Constant.Pattern.None:
                    this.Show = this.MetaInfo;
                    break;
                case Models.Constant.Pattern.Bilingual:
                    this.Show = this.MetaInfo;
                    break;
                case Models.Constant.Pattern.English:
                    this.Show = this.Interpretioin;
                    break;
                case Models.Constant.Pattern.Chinese:
                    this.Show = this.Word;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 朗读单词
        /// </summary>
        public virtual void Speak()
        {
            switch (this.Voice)
            {
                case Models.Constant.Voice.Local:
                    Speaker.ReadStringAsync(this.Word);
                    break;
                case Models.Constant.Voice.USA:
                    if (this.Meta.VoiceUSA !=null)
                    {
                        Speaker.ReadBytesAsync(this.Meta.VoiceUSA);
                    }
                    else
                    {
                        Speaker.ReadStringAsync(this.Word);
                    }
                    break;
                case Models.Constant.Voice.UK:
                    if (this.Meta.VoiceUK != null)
                    {
                        Speaker.ReadBytesAsync(this.Meta.VoiceUK);
                    }
                    else
                    {
                        Speaker.ReadStringAsync(this.Word);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        public override string ToString() => this.Meta.ToString();
    }
}

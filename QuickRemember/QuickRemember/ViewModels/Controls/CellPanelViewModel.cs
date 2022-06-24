using QuickRemember.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QuickRemember.ViewModels.Controls
{
    public class CellPanelViewModel : Core.MetaWordViewModel
    {
        #region Timer
        DispatcherTimer timer;
        public double Interval { get; set; } = 2;

        /// <summary>
        /// 定时显示内容还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTimerTick(object? sender, EventArgs e)
        {
            this.ShowFront();
            this.timer.Stop();
        }
        #endregion

        #region Action

        /// <summary>
        /// 单击切换中英文
        /// Interval之后自动回默认结果
        /// </summary>
        public void LeftButtonClick()
        {
            if (!this.timer.IsEnabled)
            {
                ShowBack();
                this.timer.Start();
            }
            else // 定时器已经启动了
            {
                ShowFront();
                this.timer.Stop();
            }
        }

        /// <summary>
        /// 朗读单词
        /// </summary>
        public void RightButtonClick()
            => Speak();


        public void MiddleButtonClick()
            => System.Windows.MessageBox.Show(this.ToString());


        #endregion

        public CellPanelViewModel(MetaWord meta) : base(meta)
        {
            timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(this.Interval)
            };

            timer.Tick += ShowTimerTick;
        }

        public override void Refresh()
        {
            this.timer?.Stop();
            this.ShowFront();
        }


    }
}

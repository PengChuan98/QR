using QuickRemember.Models.Core;
using QuickRemember.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Translator
{
    public abstract class BaseTranslator
    {
        /// <summary>
        /// 单词数据集合
        /// </summary>
        protected List<MetaWord>? WordCollection { get; set; }=null;

        /// <summary>
        /// 配合异步用的队列
        /// </summary>
        protected Queue<MetaWord>? WordQueue { get; private set; } = null;

        public List<string>? ErrorMessageCollection { get; set; } = null;

        private int TotalCount { get; set; } = 0;

        /// <summary>
        /// 剩余任务量
        /// </summary>
        public int TaskCount { get; set; } = 0;

        /// <summary>
        /// 成功数
        /// </summary>
        public int SuccessCount { get; set; } = 0;

        /// <summary>
        /// 失败数
        /// </summary>
        public int FailedCount { get; set; } = 0;

        /// <summary>
        /// 对未完成内容更新的重复次数
        /// </summary>
        public int RepeatCount { get; set; } = 1;

        /// <summary>
        /// 是否强制刷新所有内容
        /// </summary>
        public bool IsForced { get; set; } = false;

        /// <summary>
        /// 任务完成度百分比
        /// </summary>
        public double Percentage { get => 1 - ((double)this.TaskCount / (double)this.TotalCount); }

        /// <summary>
        /// 任务完成度，Percentage只和成功数有关
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        protected abstract Task ParserAsync(MetaWord word, string html);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        protected abstract string GenerateURL(string word);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        public BaseTranslator(List<MetaWord> words) => this.WordCollection = words;

        /// <summary>
        /// 初始化数据并且收集内容
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected virtual void CollectionData()
        {
            this.WordQueue = new();
            this.ErrorMessageCollection = new();

            if (this.WordCollection == null) throw new Exception("单词数据集为空");

            this.WordCollection.ForEach(word =>
            {
                if (this.IsForced)
                {
                    this.WordQueue.Enqueue(word);
                }
                else
                {
                    if (!word.IsTrans)
                    {
                        this.WordQueue.Enqueue(word);
                    }
                }
            });

            this.TotalCount = this.WordCollection.Count;
            this.TaskCount = this.WordCollection.Count;

            this.SuccessCount = 0;
            this.FailedCount = 0;

            this.IsCompleted = false;
        }

        /// <summary>
        /// 收集未完成的单词
        /// </summary>
        protected virtual void CollectionFailedData()
        {
            this.WordQueue = new();

            this.WordCollection?.ForEach(word =>
            {
                if (word.IsTrans)
                {
                    this.WordQueue.Enqueue(word);
                }
            });
        }

        /// <summary>
        /// 异步下载单个内容
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SingleDownloadAsync()
        {
            // 做一个隔断
            if (this.WordQueue == null || this.WordQueue.Count == 0) return;

            MetaWord word = this.WordQueue.Dequeue();

            string url = GenerateURL(word.Word);

            // 开始解析结果
            try
            {
                string content = await ClientHelper.DownloadHTMLAsync(url);
                await this.ParserAsync(word, content);

                // 这个单词无错获取数据
                word.IsTrans = true;
                SuccessCount++;
                return;
            }
            catch (Exception e)
            {
                word.IsTrans = false;
                this.ErrorMessageCollection?.Add("[BaseTranslator] - [SingleDownloadAsync] " + e.Message);
                FailedCount++;
            }
            finally
            {
                // 无论是否成功，任务数都减1
                this.TaskCount--;
            }
        }

        /// <summary>
        /// 异步下载所有数据
        /// </summary>
        public async Task DownloadAsync(int repeat = -1)
        {
            RepeatCount = repeat > 0 ? repeat : RepeatCount;

            // 收集需要的数据
            this.CollectionData();

            List<Task> taskCaller = new();

            for (int i = 0; i < WordCollection?.Count; i++)
            {
                taskCaller.Add(SingleDownloadAsync());
            }

            await Task.WhenAll(taskCaller);

            if (this.SuccessCount!=0)
            {
                for (int i = 0; i < this.RepeatCount; i++)
                {
                    await DownloadFailedAsync();
                }
            }

            this.IsCompleted = true;
        }

        /// <summary>
        /// 重新下载未下载成功的部分
        /// </summary>
        /// <returns></returns>
        protected virtual async Task DownloadFailedAsync()
        {
            this.CollectionFailedData();

            List<Task> taskCaller = new();
            for (int i = 0; i < this.WordQueue?.Count; i++)
            {
                taskCaller.Add(this.SingleDownloadAsync());
            }
            await Task.WhenAll(taskCaller);
        }
    }
}

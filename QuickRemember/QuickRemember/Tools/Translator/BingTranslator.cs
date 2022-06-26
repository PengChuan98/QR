using HtmlAgilityPack;
using QuickRemember.Models.Core;
using QuickRemember.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Translator
{
    public class BingTranslator : BaseTranslator
    {
        /// <summary>
        /// Bing字典解析器
        /// </summary>
        /// <param name="metas"></param>
        public BingTranslator(List<MetaWord> metas) : base(metas) { }

        /* Bug
         * 在开启VPN之后，这里的词典访问不到了，目前猜测是cookie的问题
         */

        /// <summary>
        /// 依据Word生成我需要的URL访问地址
        /// </summary>
        /// <returns></returns>
        protected override string GenerateURL(string word) => string.Format("https://cn.bing.com/dict/search?q={0}", word);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        protected override async Task ParserAsync(MetaWord word, string html)
        {
            try
            {
                HtmlDocument builder = new();
                builder.LoadHtml(html);

                HtmlNode mainNode = builder.DocumentNode.SelectSingleNode("//*/div[@class='qdef']");
                HtmlNode headNode = mainNode.SelectSingleNode("div[@class=\"hd_area\"]");

                HtmlNodeCollection interpretionCollection = mainNode.SelectNodes("ul/li");

                word.WebInterpretion = "";
                // 获取解释

                Dictionary<string, string> dict = new();
                foreach (HtmlNode node in interpretionCollection)
                {
                    string k = node.SelectSingleNode("span[@class=\"pos\"]|span[@class=\"pos web\"]").InnerText;
                    k = k.Contains("网络") ? "web" : k;
                    string v = node.SelectSingleNode("span[@class=\"def b_regtxt\"]/span").InnerText;
                    dict[k] = v;
                }
                word.WebInterpretion = string.Join("\n", dict.Select(x => $"{x.Key}:{x.Value}"));
                // 音标和读音的父节点
                HtmlNode speakerNode = builder.DocumentNode.SelectSingleNode("//*/div[@class='hd_p1_1']");

                //! 突然发现读音没什么有大用处
                //// 获取两个音标
                //HtmlNode phoneticsNode1 = speakerNode.SelectSingleNode("div[@class=\"hd_prUS b_primtxt\"]");
                //word.PhoneticsUSA = phoneticsNode1.InnerText.Replace("&nbsp;", " ").Replace("&#160;", " ").Trim();

                //HtmlNode phoneticsNode2 = speakerNode.SelectSingleNode("div[@class=\"hd_pr b_primtxt\"]");
                //word.PhoneticsUK = phoneticsNode2.InnerText.Replace("&nbsp;", " ").Replace("&#160;", " ").Trim();

                // 获取两个读音
                Regex voiceUrlRegex = new(@"http[s]?://(?:[a-zA-Z]|[0-9]|[$-_@.&+]|[!*\(\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+\.mp3");
                HtmlNodeCollection voiceNode = speakerNode.SelectNodes("div[@class=\"hd_tf\"]");
                var voice1 = voiceUrlRegex.Match(voiceNode[0].InnerHtml).Value;
                var voice2 = voiceUrlRegex.Match(voiceNode[1].InnerHtml).Value;

                word.VoiceUK = await ClientHelper.DownloadBytesAsync(voice2);
                word.VoiceUSA = await ClientHelper.DownloadBytesAsync(voice1);

                word.IsTrans = true;
            }
            catch (Exception e)
            {
                this.ErrorMessageCollection?.Add("[BingTranslator] - [ParserAsync] " + e.ToString());
                word.IsTrans = false;
            }
        }
    }
}

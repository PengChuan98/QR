using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Helper
{
    public class ClientHelper
    {
        static HttpClient client = new HttpClient();

        /// <summary>
        /// 下载网页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DownloadHTMLAsync(string url)
        {
            string content = "";
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("",e);
            }
            return content;
        }

        /// <summary>
        /// 下载二进制网络资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]?> DownloadBytesAsync(string url)
        {
            byte[]? content=null;
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception e)
            {
                throw new Exception("",e);
            }
            return content;
        }
    }
}

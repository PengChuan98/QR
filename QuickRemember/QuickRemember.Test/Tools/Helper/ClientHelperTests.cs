using QuickRemember.Tools.Helper;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace QuickRemember.Test.Tools.Helper
{
    [TestClass]
    public class ClientHelperTests
    {
        [TestMethod]
        public void CanCallDownloadHTMLAsync()
        {
            // Arrange
            var url = "https://www.baidu.com";

            // Act
            Task<string> task = ClientHelper.DownloadHTMLAsync(url);
            task.Wait();

            // Assert
            Assert.AreNotEqual(task.Result, "");
        }

        [TestMethod]
        public void CannotCanCallDownloadHTMLAsync()
        {
            // Arrange
            var url = "https://www.baidu";

            // Assert
            Assert.ThrowsException<AggregateException>(() =>
            {
                Task<string> task = ClientHelper.DownloadHTMLAsync(url);
                task.Wait();
            });
        }

        [TestMethod]
        public void CanCallDownloadBytesAsync()
        {
            // Arrange
            var url = "https://www.baidu.com";

            // Act
            Task<byte[]> task = ClientHelper.DownloadBytesAsync(url);
            task.Wait();

            // Assert
            Assert.AreNotEqual(task.Result, new byte[] {});
        }
    }
}
using QuestPDF.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Generator.PDF.Core
{
    public abstract class DocumentViewBase
    {
        /// <summary>
        /// 生产默认的元信息
        /// </summary>
        /// <param name="author"></param>
        /// <param name="title"></param>
        /// <param name="subject"></param>
        /// <param name="keyword"></param>
        /// <param name="creator"></param>
        /// <param name="producer"></param>
        /// <returns></returns>
        public DocumentMetadata ConfigMetadata(
            string author = "QR",
            string title = "QR Title",
            string subject = "",
            string keyword = "QR",
            string creator = "QR",
            string producer = "PengChuan")
        {
            DocumentMetadata Metadata;
            Metadata = new DocumentMetadata();
            Metadata.Author = author;
            Metadata.Title = title;
            Metadata.Subject = subject;
            Metadata.Keywords = keyword;
            Metadata.Creator = creator;
            Metadata.Producer = producer;
            return Metadata;
        }

        public abstract void Render(string path);
    }
}

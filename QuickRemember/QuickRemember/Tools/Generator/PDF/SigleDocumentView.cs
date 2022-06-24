using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Collections.Generic;
using QuickRemember.Tools.Generator.PDF.Core;

// TODO 这里要重写

namespace QuickRemember.Tools.Generator.PDF
{
    public class SigleDocumentView : DocumentViewBase
    {
        public string Content { get; set; } = "";
        public string FontFamily { get; set; } = "SimSun";
        public int FontSize { get; set; } = 12;
        public double Margin { get; set; } = 10;
        public float Border { get; set; } = 10f;

        public bool IsLandscape = false;
        public float Padding { get; set; }

        public float Height
        {
            get => !IsLandscape ? PageSizes.A4.Height : PageSizes.A4.Width;
        }

        public float Width
        {
            get => !IsLandscape ? PageSizes.A4.Width : PageSizes.A4.Height;
        }

        PageModel page;
        GridModel grid;

        TableModel table;
        List<TableModel> tables;

        DocumentViewModel document;  

        /// <summary>
        /// Initializes a new instance of the <see cref="SigleDocumentView"/> class.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="border"></param>
        /// <param name="margin"></param>
        /// <param name="padding"></param>
        /// <param name="isLandscape"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        public SigleDocumentView(string content, float border = 0f,
            float margin = 10f, float padding = 20f,
            bool isLandscape = false,
            string fontFamily = "Times New Roman", int fontSize = 20)
        {
            this.Content = content;
            page = new();
            grid = new();

            table = new();
            table.TableDefaultFontSize = 100;

            // 初始值
            this.Border = border;
            this.Margin = margin;
            this.Padding = padding;
            this.IsLandscape = isLandscape;

            this.FontFamily = fontFamily;
            this.FontSize = fontSize;

        }

        void LoadModel()
        {
            // page
            page.Margin = new System.Windows.Thickness(this.Margin);

            // grid
            grid.RowDefinitions = 1;
            grid.ColumnDefinitions = 1;
            grid.Border = this.Border;

            table.Data = new string[] { this.Content };
            table.RowDefinitions = 1;
            table.ColumnDefinitions = 1;

            // 这里把Border暂借作Padding用
            table.Border = Padding;

            table.TableDefaultFontFamily = this.FontFamily;
            table.TableDefaultFontSize = this.FontSize;
            tables = new List<TableModel>();
            tables.Add(table);
        }

        /// <summary>
        /// 生产PDF
        /// </summary>
        /// <param name="path"></param>
        public override void Render(string path)
        {
            LoadModel();

            document = new DocumentViewModel(page, grid, tables, ConfigMetadata());
            document.GeneratePdf(path);
        }
    }
}

using QuickRemember.Tools.Translator;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using QuickRemember.Models.Core;
using QuickRemember.Tools.Helper;

namespace QuickRemember.Test.Tools.Translator
{
    [TestClass]
    public class BingTranslatorTests
    {
        private BingTranslator _testClass;
        private List<MetaWord> _metas;

        [TestInitialize]
        public void SetUp()
        {
            _metas = new List<MetaWord>();
            CSVHelper.Parse(CSVHelper.LoadCSVFile(@"E:\ÊÓÆµ\Ó¢Óï\±Ø¿¼-ÂÒÐò.csv"), 2).ForEach(line =>
            {
                _metas.Add(new(line[0], line[1]));
            });

            _testClass = new BingTranslator(ListHelper<MetaWord>.SplitList(_metas,20,1));
        }

        [TestMethod]
        public void CanRunAsync()
        {
            // Act
            var instance = _testClass;

            // Assert
            Task task = instance.DownloadAsync();
            task.Wait();

            Console.WriteLine(string.Join("\n", _testClass.ErrorMessageCollection));
            Console.WriteLine(_metas[0].WebInterpretion);
        }
    }
}
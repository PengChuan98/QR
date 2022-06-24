using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRemember.Tools.Helper
{
    public static class DialogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool OpenFileDialog(ref string path, string filter= "CSV Files (*.csv)|*.csv|(*.*)|*.*")
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = filter,
            };

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                path = openFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}

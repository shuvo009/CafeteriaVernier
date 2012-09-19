using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;

namespace Procesta.CvServer
{
    class CommonInfoSearch
    {
        public CommonInfoSearch(ICollectionView filteredList, TextBox textEdit)
        {
            string filterText = string.Empty;

            filteredList.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                {
                    return true;
                }
                ModelCommonUse str = obj as ModelCommonUse;
                if (str.UserName == null)
                {
                    return true;
                }
                if (str.UserName.Trim().ToUpper().Contains(filterText.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            textEdit.TextChanged += delegate
            {
                filterText = textEdit.Text;
                filteredList.Refresh();
            };
        }
    }
}

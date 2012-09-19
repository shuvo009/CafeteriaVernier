using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;

namespace Procesta.CvServer
{
    class CustomerInfoSearch
    {
        public CustomerInfoSearch(ICollectionView filteredList, TextBox textEdit)
        {
            string filterText = string.Empty;
            
            filteredList.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                {
                    return true;
                }
                ModelCustomer str = obj as ModelCustomer;
                if (str.UserName==null)
                {
                    return true;
                }
                if (str.UserName.ToUpper().Contains(filterText.ToUpper()))
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

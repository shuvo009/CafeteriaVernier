using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;

namespace Procesta.CvServer
{
    class AccountRechargeSearchs
    {
        public AccountRechargeSearchs(ICollectionView filteredList, TextBox textEdit)
        {
            string filterText = string.Empty;
            string properyText = string.Empty;
            filteredList.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                {
                    return true;
                }
                if (obj is ModelCustomer)
                {
                    properyText = (obj as ModelCustomer).Name;
                }
                if (obj is ModelTeamInfo)
                {
                    properyText = (obj as ModelTeamInfo).Name;
                }
                if (String.IsNullOrEmpty(properyText))
                {
                    return true;
                }
                if (properyText.ToUpper().Contains(filterText.ToUpper()))
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

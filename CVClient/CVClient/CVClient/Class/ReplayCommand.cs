using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DevExpress.Xpf.Core;

namespace Procesta.CVClient.Class
{
    public class ReplayCommand : ICommand
    {
        private Action<object> _action;
        public ReplayCommand(Action<object> action)
        {
            this._action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        #pragma warning disable 67
        public event EventHandler CanExecuteChanged;
        #pragma warning disable 67

        public void Execute(object parameter)
        {
            try
            {
                if (parameter!=null)
                {
                    this._action(parameter);
                }
                else
                {
                    DXMessageBox.Show("Parameter can`t be Null", CVsVariables.SOTWARE_NAME, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message, CVsVariables.SOTWARE_NAME, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}

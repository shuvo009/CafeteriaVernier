using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Net;
using DevExpress.Xpf.Core;
using Procesta.CVClient.Class.Exceptions;
using Procesta.CVClient.Class;
using System.Diagnostics;
using Procesta.CVClient.Class.Methords;
namespace Procesta.CVClient
{
    /// <summary>
    /// Interaction logic for InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        public InstallWindow()
        {
            InitializeComponent();
        }

        public ICommand UpdateButtonCommand
        {
            get 
            {
                return new ReplayCommand(new Action<object>(this.UpdateButton_Click));
            }
        }

        private void UpdateButton_Click(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Properties.Settings.Default.IsConfigered = true;
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown(); 
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                DXMessageBox.Show(error.Message, CVsVariables.ERROR_MESSAGES[0, 0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    
        /// <summary>
        /// Window Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpAddressWindowsLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IPAddress[] localPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress WorkingIp in localPs)
            {
                this.IpAddressComBoBox.Items.Add(WorkingIp);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace CvAutoBackup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                this.startupEntry();
                if (e.Args.Count() > 0 && e.Args[0].ToString().Equals("-h"))
                {
                    MainWindow sysTryMainWindo = new MainWindow();
                   
                    sysTryMainWindo.Show(); 
                    sysTryMainWindo.WindowState = WindowState.Minimized;
                }
                else
                {
                    new MainWindow().Show();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Variables.ERROR_MESSAGES[0], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void startupEntry()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (regKey != null)
                {
                    object keyValue = regKey.GetValue("cvAutoBackup");
                    if (keyValue == null)
                    {
                        Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true).SetValue("cvAutoBackup",string.Format("{0} -h",System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String));
                    }
                }
            }
        }
    }
}

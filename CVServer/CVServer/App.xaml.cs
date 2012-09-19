using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Procesta.CvServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Cv_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen CvSplash = new SplashScreen("Images/SplashScreen.png");
            CvSplash.Show(false);
            CvSplash.Close(TimeSpan.FromSeconds(0.9));
        }
    }
}

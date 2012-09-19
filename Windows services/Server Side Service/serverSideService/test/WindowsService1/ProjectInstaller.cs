using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Procesta.serverSideService.Class.Methords;


namespace WindowsService1
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            AfterInstall += new InstallEventHandler(ProjectInstaller_AfterInstall);
        }

        void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            MiraculousMethod.ConnectionStringChanger();

        }
    }
}

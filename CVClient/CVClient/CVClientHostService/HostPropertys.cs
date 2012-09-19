using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Xml.Linq;

namespace CVClientHostService
{
    public class HostPropertys
    {
        public static string HostIpAddress
        {
            get
            {
                return Properties.Settings.Default.HostIp;
            }
            set
            {
                try
                {
                    string configPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CVClientHostService.exe.config");
                    var lodCon = XDocument.Load(configPath);
                    lodCon.Descendants("value").Remove();
                    lodCon.Descendants("setting").SingleOrDefault().SetElementValue("value", value);
                    lodCon.Save(configPath);
                }
                catch
                {
                    throw new Exception("Unable Save Server IP");
                }
            }
        }
    }
}

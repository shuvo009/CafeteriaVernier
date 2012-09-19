using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesta.CvServer.Class;
namespace Procesta.CvServer
{
    /// <summary>
    /// Create a list to this class to hold all permission.ModelEmployPermissions
    /// </summary>
    [Serializable]
    public class ModelEmployPermissions
    {
        public ModelEmployPermissions()
        {

        }
        public string Item { get; set; }
        public string Setting { get; set; }
        public bool Permission { get; set; }
        public int Priority { get; set; }
        public string SupperTip { get; set; }
        public string ScreenTip { get; set; }
        public string KeboardShortcut { get; set; }
        public string ImagePath { get; set; }
    }
}

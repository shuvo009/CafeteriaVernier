using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.Win32;

namespace Procesta.ClientSideService.Class.Property
{
   public class ResistryProperty
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public string keyName { get; set; }

        public string enableValue { get; set; }

        public string disableValue { get; set; }

        public RegistryValueKind valueKind { get; set; }

    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Procesta.CvServer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string schreenCapturePath {
            get {
                return ((string)(this["schreenCapturePath"]));
            }
            set {
                this["schreenCapturePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IsCupture {
            get {
                return ((bool)(this["IsCupture"]));
            }
            set {
                this["IsCupture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30.00:00:00")]
        public global::System.TimeSpan CuptureTime {
            get {
                return ((global::System.TimeSpan)(this["CuptureTime"]));
            }
            set {
                this["CuptureTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string settingEmail {
            get {
                return ((string)(this["settingEmail"]));
            }
            set {
                this["settingEmail"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string settingEmailPassword {
            get {
                return ((string)(this["settingEmailPassword"]));
            }
            set {
                this["settingEmailPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string ServerIpAddress {
            get {
                return ((string)(this["ServerIpAddress"]));
            }
            set {
                this["ServerIpAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsConfigered {
            get {
                return ((bool)(this["IsConfigered"]));
            }
            set {
                this["IsConfigered"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("data source=.\\SQLEXPRESS;initial catalog=Cafeteria_Vernier_db;integrated security" +
            "=True")]
        public string Cafeteria_Vernier_dbConnectionString {
            get {
                return ((string)(this["Cafeteria_Vernier_dbConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Program Files\\Microsoft SQL Server\\MSSQL10.SQLEXPRESS\\MSSQL\\DATA")]
        public string SqlDataFolder {
            get {
                return ((string)(this["SqlDataFolder"]));
            }
            set {
                this["SqlDataFolder"] = value;
            }
        }
    }
}

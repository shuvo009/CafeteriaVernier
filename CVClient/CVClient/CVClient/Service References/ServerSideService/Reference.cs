﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Procesta.CVClient.ServerSideService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Userinformation", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class Userinformation : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> DateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NationalIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] UserImageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address {
            get {
                return this.AddressField;
            }
            set {
                if ((object.ReferenceEquals(this.AddressField, value) != true)) {
                    this.AddressField = value;
                    this.RaisePropertyChanged("Address");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> Date {
            get {
                return this.DateField;
            }
            set {
                if ((this.DateField.Equals(value) != true)) {
                    this.DateField = value;
                    this.RaisePropertyChanged("Date");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NationalID {
            get {
                return this.NationalIDField;
            }
            set {
                if ((object.ReferenceEquals(this.NationalIDField, value) != true)) {
                    this.NationalIDField = value;
                    this.RaisePropertyChanged("NationalID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Phone {
            get {
                return this.PhoneField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneField, value) != true)) {
                    this.PhoneField = value;
                    this.RaisePropertyChanged("Phone");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] UserImage {
            get {
                return this.UserImageField;
            }
            set {
                if ((object.ReferenceEquals(this.UserImageField, value) != true)) {
                    this.UserImageField = value;
                    this.RaisePropertyChanged("UserImage");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoginHistory", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class LoginHistory : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> CounterNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> EntryDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> MinutesUseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TeamNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> CounterNumber {
            get {
                return this.CounterNumberField;
            }
            set {
                if ((this.CounterNumberField.Equals(value) != true)) {
                    this.CounterNumberField = value;
                    this.RaisePropertyChanged("CounterNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> EntryDate {
            get {
                return this.EntryDateField;
            }
            set {
                if ((this.EntryDateField.Equals(value) != true)) {
                    this.EntryDateField = value;
                    this.RaisePropertyChanged("EntryDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> MinutesUse {
            get {
                return this.MinutesUseField;
            }
            set {
                if ((this.MinutesUseField.Equals(value) != true)) {
                    this.MinutesUseField = value;
                    this.RaisePropertyChanged("MinutesUse");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TeamName {
            get {
                return this.TeamNameField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamNameField, value) != true)) {
                    this.TeamNameField = value;
                    this.RaisePropertyChanged("TeamName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserRechareHistory", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class UserRechareHistory : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<decimal> AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> EntryDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> MunitiesField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployID {
            get {
                return this.EmployIDField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployIDField, value) != true)) {
                    this.EmployIDField = value;
                    this.RaisePropertyChanged("EmployID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> EntryDate {
            get {
                return this.EntryDateField;
            }
            set {
                if ((this.EntryDateField.Equals(value) != true)) {
                    this.EntryDateField = value;
                    this.RaisePropertyChanged("EntryDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Munities {
            get {
                return this.MunitiesField;
            }
            set {
                if ((this.MunitiesField.Equals(value) != true)) {
                    this.MunitiesField = value;
                    this.RaisePropertyChanged("Munities");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AllUserAndTeam", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class AllUserAndTeam : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AddOrDeleteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool AddOrDelete {
            get {
                return this.AddOrDeleteField;
            }
            set {
                if ((this.AddOrDeleteField.Equals(value) != true)) {
                    this.AddOrDeleteField = value;
                    this.RaisePropertyChanged("AddOrDelete");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TeamInfo", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class TeamInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime EntryDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TeamAdminField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TeamDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] TeamImageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short TotalMemberField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime EntryDate {
            get {
                return this.EntryDateField;
            }
            set {
                if ((this.EntryDateField.Equals(value) != true)) {
                    this.EntryDateField = value;
                    this.RaisePropertyChanged("EntryDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TeamAdmin {
            get {
                return this.TeamAdminField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamAdminField, value) != true)) {
                    this.TeamAdminField = value;
                    this.RaisePropertyChanged("TeamAdmin");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TeamD {
            get {
                return this.TeamDField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamDField, value) != true)) {
                    this.TeamDField = value;
                    this.RaisePropertyChanged("TeamD");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] TeamImage {
            get {
                return this.TeamImageField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamImageField, value) != true)) {
                    this.TeamImageField = value;
                    this.RaisePropertyChanged("TeamImage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short TotalMember {
            get {
                return this.TotalMemberField;
            }
            set {
                if ((this.TotalMemberField.Equals(value) != true)) {
                    this.TotalMemberField = value;
                    this.RaisePropertyChanged("TotalMember");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="counterSetting", Namespace="http://schemas.datacontract.org/2004/07/Procesta.serverSideService.Class.Property" +
        "es")]
    [System.SerializableAttribute()]
    public partial class counterSetting : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int secondsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int seconds {
            get {
                return this.secondsField;
            }
            set {
                if ((this.secondsField.Equals(value) != true)) {
                    this.secondsField = value;
                    this.RaisePropertyChanged("seconds");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServerSideService.IServerSideServices")]
    public interface IServerSideServices {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserLogin", ReplyAction="http://tempuri.org/IServerSideServices/UserLoginResponse")]
        System.Collections.Generic.List<long> UserLogin(string username, string password, short counterNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamLogin", ReplyAction="http://tempuri.org/IServerSideServices/TeamLoginResponse")]
        System.Collections.Generic.List<long> TeamLogin(string username, string password, short counterNumber, string teamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserMunitieCounter", ReplyAction="http://tempuri.org/IServerSideServices/UserMunitieCounterResponse")]
        bool UserMunitieCounter(string username, short counterNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamMunitieCounter", ReplyAction="http://tempuri.org/IServerSideServices/TeamMunitieCounterResponse")]
        bool TeamMunitieCounter(string teamName, string username, short counterNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/AccountDetails", ReplyAction="http://tempuri.org/IServerSideServices/AccountDetailsResponse")]
        Procesta.CVClient.ServerSideService.Userinformation AccountDetails(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/AccountUpdate", ReplyAction="http://tempuri.org/IServerSideServices/AccountUpdateResponse")]
        bool AccountUpdate(string username, Procesta.CVClient.ServerSideService.Userinformation updateinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserLoginHistoryAll", ReplyAction="http://tempuri.org/IServerSideServices/UserLoginHistoryAllResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryAll(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserLoginHistoryDate", ReplyAction="http://tempuri.org/IServerSideServices/UserLoginHistoryDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryDate(string username, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserLoginHistoryTwoDate", ReplyAction="http://tempuri.org/IServerSideServices/UserLoginHistoryTwoDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryTwoDate(string username, System.DateTime firstDate, System.DateTime secondDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserRechargeHistoryAll", ReplyAction="http://tempuri.org/IServerSideServices/UserRechargeHistoryAllResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryAll(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserRechargeHistoryDate", ReplyAction="http://tempuri.org/IServerSideServices/UserRechargeHistoryDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryDate(string username, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserRechargeHistoryTwoDate", ReplyAction="http://tempuri.org/IServerSideServices/UserRechargeHistoryTwoDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryTwoDate(string username, System.DateTime firstDate, System.DateTime secondDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamRechargeHistoryAll", ReplyAction="http://tempuri.org/IServerSideServices/TeamRechargeHistoryAllResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryAll(string TeamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamRechargeHistoryDate", ReplyAction="http://tempuri.org/IServerSideServices/TeamRechargeHistoryDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryDate(string TeamName, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamRechargeHistoryTwoDate", ReplyAction="http://tempuri.org/IServerSideServices/TeamRechargeHistoryTwoDateResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryTwoDate(string TeamName, System.DateTime firstDate, System.DateTime secondDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetAllUsers", ReplyAction="http://tempuri.org/IServerSideServices/GetAllUsersResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetAllUsers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamNameChecker", ReplyAction="http://tempuri.org/IServerSideServices/TeamNameCheckerResponse")]
        bool TeamNameChecker(string teamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/AddNewTeam", ReplyAction="http://tempuri.org/IServerSideServices/AddNewTeamResponse")]
        bool AddNewTeam(Procesta.CVClient.ServerSideService.TeamInfo teamInfo, System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> members);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/DeleteMember", ReplyAction="http://tempuri.org/IServerSideServices/DeleteMemberResponse")]
        bool DeleteMember(string teamName, Procesta.CVClient.ServerSideService.AllUserAndTeam member);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/CounterSettings", ReplyAction="http://tempuri.org/IServerSideServices/CounterSettingsResponse")]
        Procesta.CVClient.ServerSideService.counterSetting CounterSettings();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetUserImage", ReplyAction="http://tempuri.org/IServerSideServices/GetUserImageResponse")]
        byte[] GetUserImage(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetTeamName", ReplyAction="http://tempuri.org/IServerSideServices/GetTeamNameResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetTeamName(string adminName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UpdateTeam", ReplyAction="http://tempuri.org/IServerSideServices/UpdateTeamResponse")]
        bool UpdateTeam(string teamName, System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> teamMembers);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UserLogout", ReplyAction="http://tempuri.org/IServerSideServices/UserLogoutResponse")]
        bool UserLogout(string username, long loginHistoryID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/TeamLogout", ReplyAction="http://tempuri.org/IServerSideServices/TeamLogoutResponse")]
        bool TeamLogout(string username, string teamName, long loginHistoryID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetTeamMember", ReplyAction="http://tempuri.org/IServerSideServices/GetTeamMemberResponse")]
        System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetTeamMember(string teamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetUserBalance", ReplyAction="http://tempuri.org/IServerSideServices/GetUserBalanceResponse")]
        int GetUserBalance(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetTeamBalance", ReplyAction="http://tempuri.org/IServerSideServices/GetTeamBalanceResponse")]
        int GetTeamBalance(string teamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/PasswordChange", ReplyAction="http://tempuri.org/IServerSideServices/PasswordChangeResponse")]
        bool PasswordChange(string username, string newPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/GetTeamLogo", ReplyAction="http://tempuri.org/IServerSideServices/GetTeamLogoResponse")]
        byte[] GetTeamLogo(string teamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerSideServices/UpdateTeamLogo", ReplyAction="http://tempuri.org/IServerSideServices/UpdateTeamLogoResponse")]
        void UpdateTeamLogo(string teamName, byte[] teamLogo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerSideServicesChannel : Procesta.CVClient.ServerSideService.IServerSideServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerSideServicesClient : System.ServiceModel.ClientBase<Procesta.CVClient.ServerSideService.IServerSideServices>, Procesta.CVClient.ServerSideService.IServerSideServices {
        
        public ServerSideServicesClient() {
        }
        
        public ServerSideServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServerSideServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerSideServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerSideServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<long> UserLogin(string username, string password, short counterNumber) {
            return base.Channel.UserLogin(username, password, counterNumber);
        }
        
        public System.Collections.Generic.List<long> TeamLogin(string username, string password, short counterNumber, string teamName) {
            return base.Channel.TeamLogin(username, password, counterNumber, teamName);
        }
        
        public bool UserMunitieCounter(string username, short counterNumber) {
            return base.Channel.UserMunitieCounter(username, counterNumber);
        }
        
        public bool TeamMunitieCounter(string teamName, string username, short counterNumber) {
            return base.Channel.TeamMunitieCounter(teamName, username, counterNumber);
        }
        
        public Procesta.CVClient.ServerSideService.Userinformation AccountDetails(string username) {
            return base.Channel.AccountDetails(username);
        }
        
        public bool AccountUpdate(string username, Procesta.CVClient.ServerSideService.Userinformation updateinfo) {
            return base.Channel.AccountUpdate(username, updateinfo);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryAll(string username) {
            return base.Channel.UserLoginHistoryAll(username);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryDate(string username, System.DateTime date) {
            return base.Channel.UserLoginHistoryDate(username, date);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.LoginHistory> UserLoginHistoryTwoDate(string username, System.DateTime firstDate, System.DateTime secondDate) {
            return base.Channel.UserLoginHistoryTwoDate(username, firstDate, secondDate);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryAll(string username) {
            return base.Channel.UserRechargeHistoryAll(username);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryDate(string username, System.DateTime date) {
            return base.Channel.UserRechargeHistoryDate(username, date);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> UserRechargeHistoryTwoDate(string username, System.DateTime firstDate, System.DateTime secondDate) {
            return base.Channel.UserRechargeHistoryTwoDate(username, firstDate, secondDate);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryAll(string TeamName) {
            return base.Channel.TeamRechargeHistoryAll(TeamName);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryDate(string TeamName, System.DateTime date) {
            return base.Channel.TeamRechargeHistoryDate(TeamName, date);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.UserRechareHistory> TeamRechargeHistoryTwoDate(string TeamName, System.DateTime firstDate, System.DateTime secondDate) {
            return base.Channel.TeamRechargeHistoryTwoDate(TeamName, firstDate, secondDate);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetAllUsers() {
            return base.Channel.GetAllUsers();
        }
        
        public bool TeamNameChecker(string teamName) {
            return base.Channel.TeamNameChecker(teamName);
        }
        
        public bool AddNewTeam(Procesta.CVClient.ServerSideService.TeamInfo teamInfo, System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> members) {
            return base.Channel.AddNewTeam(teamInfo, members);
        }
        
        public bool DeleteMember(string teamName, Procesta.CVClient.ServerSideService.AllUserAndTeam member) {
            return base.Channel.DeleteMember(teamName, member);
        }
        
        public Procesta.CVClient.ServerSideService.counterSetting CounterSettings() {
            return base.Channel.CounterSettings();
        }
        
        public byte[] GetUserImage(string username) {
            return base.Channel.GetUserImage(username);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetTeamName(string adminName) {
            return base.Channel.GetTeamName(adminName);
        }
        
        public bool UpdateTeam(string teamName, System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> teamMembers) {
            return base.Channel.UpdateTeam(teamName, teamMembers);
        }
        
        public bool UserLogout(string username, long loginHistoryID) {
            return base.Channel.UserLogout(username, loginHistoryID);
        }
        
        public bool TeamLogout(string username, string teamName, long loginHistoryID) {
            return base.Channel.TeamLogout(username, teamName, loginHistoryID);
        }
        
        public System.Collections.Generic.List<Procesta.CVClient.ServerSideService.AllUserAndTeam> GetTeamMember(string teamName) {
            return base.Channel.GetTeamMember(teamName);
        }
        
        public int GetUserBalance(string username) {
            return base.Channel.GetUserBalance(username);
        }
        
        public int GetTeamBalance(string teamName) {
            return base.Channel.GetTeamBalance(teamName);
        }
        
        public bool PasswordChange(string username, string newPassword) {
            return base.Channel.PasswordChange(username, newPassword);
        }
        
        public byte[] GetTeamLogo(string teamName) {
            return base.Channel.GetTeamLogo(teamName);
        }
        
        public void UpdateTeamLogo(string teamName, byte[] teamLogo) {
            base.Channel.UpdateTeamLogo(teamName, teamLogo);
        }
    }
}
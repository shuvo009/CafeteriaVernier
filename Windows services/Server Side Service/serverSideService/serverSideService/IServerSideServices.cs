using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Procesta.serverSideService.Class.Propertyes;
using System.Collections;
namespace Procesta.serverSideService
{
    [ServiceContract]
    public interface IServerSideServices
    {
        /// <summary>
        /// UserLogin To counter.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Munities</returns>
        [OperationContract]
        List<Int64> UserLogin(string username, string password, short counterNumber);
        /// <summary>
        /// TeamLogin To counter with a Team.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="teamName">Team Name</param>
        /// <returns>Munities</returns>
        [OperationContract]
        List<Int64> TeamLogin(string username, string password, short counterNumber, string teamName);
        /// <summary>
        /// To Count Munities.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="counterNumber">Which Counter is logged in</param>
        /// <returns></returns>
        [OperationContract]
        bool UserMunitieCounter(string username, short counterNumber);
        /// <summary>
        /// To Count Munities with a team
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="counterNumber">Counter Number</param>
        /// <param name="isTeam">true if it is a team </param>
        /// <returns>if success return true</returns>
        [OperationContract]
        bool TeamMunitieCounter(string teamName, string username, short counterNumber);
        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns> Type of Userinformation</returns>
        [OperationContract]
        Userinformation AccountDetails(string username);
        /// <summary>
        /// Update User Account information
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="updateinfo">Type of Userinformation</param>
        [OperationContract]
        bool AccountUpdate(string username, Userinformation updateinfo);
        /// <summary>
        /// Get User login History All
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Type Of UserLoginHistory </returns>
        [OperationContract]
        List<LoginHistory> UserLoginHistoryAll(string username);
        /// <summary>
        /// Get User login History By Date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="date">Date</param>
        /// <returns>Type Of UserLoginHistory</returns>
        [OperationContract]
        List<LoginHistory> UserLoginHistoryDate(string username,DateTime date);
        /// <summary>
        /// Get User login History By Between two Date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="firstDate">First Date</param>
        /// <param name="secondDate">Second Date</param>
        /// <returns>Type Of UserLoginHistory</returns>
        [OperationContract]
        List<LoginHistory> UserLoginHistoryTwoDate(string username, DateTime firstDate,DateTime secondDate);
        /// <summary>
        /// Get User Recharge History by all
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> UserRechargeHistoryAll(string username);
        /// <summary>
        /// Get User Recharge History by Date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="date">Date</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> UserRechargeHistoryDate(string username,DateTime date);  
        /// <summary>
        /// Get User Recharge History between two date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="firstDate">First Date</param>
        /// <param name="secondDate">Second Date</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> UserRechargeHistoryTwoDate(string username,DateTime firstDate,DateTime secondDate);
        /// <summary>
        /// Get Team Recharge History by all
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> TeamRechargeHistoryAll(string TeamName);
        /// <summary>
        /// Get Team Recharge History by Date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="date">Date</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> TeamRechargeHistoryDate(string TeamName, DateTime date);
        /// <summary>
        /// Get User Recharge History between two date
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="firstDate">First Date</param>
        /// <param name="secondDate">Second Date</param>
        /// <returns>Type of UserRechareHistory</returns>
        [OperationContract]
        List<UserRechareHistory> TeamRechargeHistoryTwoDate(string TeamName, DateTime firstDate, DateTime secondDate);
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>type of List<AllUser></returns>
        [OperationContract]
        List<AllUserAndTeam> GetAllUsers();
        /// <summary>
        /// Check a Team Is available or not
        /// </summary>
        /// <param name="teamName">Team Name</param>
        /// <returns></returns>
        [OperationContract]
        bool TeamNameChecker(string teamName);
        /// <summary>
        /// Add new Team
        /// </summary>
        /// <param name="teamInfo">Type Of TeamInfo</param>
        /// <param name="members">Type of List<AllUser></param>
        [OperationContract]
        bool AddNewTeam(TeamInfo teamInfo, List<AllUserAndTeam> members);
        /// <summary>
        ///Delete a Member from a team
        /// </summary>
        /// <param name="adminName">Admin Name</param>
        /// <param name="member">Type of AllUser</param>
        [OperationContract]
        bool DeleteMember(string teamName, AllUserAndTeam member);
        /// <summary>
        /// Load Counter setting
        /// </summary>
        /// <returns>Type of counterSetting</returns>
        [OperationContract]
        counterSetting CounterSettings();
        /// <summary>
        /// Get picture of a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Image in Bytes</returns>
        [OperationContract]
        byte[] GetUserImage(string username);
        /// <summary>
        /// Get Team names based one AdminName
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        [OperationContract]
        List<AllUserAndTeam> GetTeamName(string adminName);
        /// <summary>
        /// To Update Team Members
        /// </summary>
        /// <param name="teamName">Team Name</param>
        /// <param name="teamMembers">Type Of AllUserAndTeam</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateTeam(string teamName, List<AllUserAndTeam> teamMembers);
        /// <summary>
        /// User Logout To the counter
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>bool</returns>
        [OperationContract]
        bool UserLogout(string username,Int64 loginHistoryID);
        /// <summary>
        /// Team Logout from a counter
        /// </summary>
        /// <param name="username">string username</param>
        /// <param name="teamName">string teamName</param>
        /// <returns>bool</returns>
        [OperationContract]
        bool TeamLogout(string username, string teamName,Int64 loginHistoryID);
        /// <summary>
        /// Get all Member of Team
        /// </summary>
        /// <param name="teamName">string TeamName</param>
        /// <returns></returns>
        [OperationContract]
        List<AllUserAndTeam> GetTeamMember(string teamName);
        /// <summary>
        /// Get User Account Balance
        /// </summary>
        /// <param name="username">string Username</param>
        /// <returns></returns>
        [OperationContract]
        int GetUserBalance(string username);
        /// <summary>
        /// Get Team Balance
        /// </summary>
        /// <param name="teamName">string TeamName</param>
        /// <returns></returns>
        [OperationContract]
        int GetTeamBalance(string teamName);
        /// <summary>
        /// To Change a User Password
        /// </summary>
        /// <param name="username">String Username</param>
        /// <param name="newPassword">String New Password</param>
        /// <returns></returns>
        [OperationContract]
        bool PasswordChange(string username, string newPassword);

        [OperationContract]
        byte[] GetTeamLogo(string teamName);

        [OperationContract]
        void UpdateTeamLogo(string teamName, byte[] teamLogo);
    }
}

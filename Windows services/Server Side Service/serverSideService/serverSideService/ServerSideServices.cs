using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Procesta.serverSideService.Class;
using Procesta.serverSideService.Class.Propertyes;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Procesta.serverSideService.Class.EntityFramework;
using System.Collections;
namespace Procesta.serverSideService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ServerSideServices : IServerSideServices
    {
        private static readonly object signalEntry = new object();

        public List<Int64> UserLogin(string username, string password, short counterNumber)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    var loginfo = cvDatabase.CustomerAccounts.FirstOrDefault(x=>x.UserID.Equals(username)
                                                                                    && x.Password.Equals(password) 
                                                                                        && x.Minutes>1 
                                                                                            && x.Status==false);
                    if (loginfo!=null)
                    {
                        UserLoginHistory newLoginHistory= new UserLoginHistory
                                                                              {
                                                                                  AutoInc= default(long),
                                                                                  CounterNumber=counterNumber,
                                                                                  StratTime=DateTime.Now,
                                                                                  UserID=username,
                                                                              };
                        cvDatabase.AddToUserLoginHistories(newLoginHistory);                                                                              
                        loginfo.Status=true;
                        loginfo.Counternumber=counterNumber;
                        cvDatabase.SaveChanges();
                        List<Int64> customerinfo = new List<Int64>();
                        customerinfo.Add(loginfo.Minutes);
                        customerinfo.Add(newLoginHistory.AutoInc);
                        return customerinfo;
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 0]);
                    }
                }
            }
            catch (Exception error)
            {
                throw new FaultException(error.Message);
            }
        }

        public List<Int64> TeamLogin(string username, string password, short counterNumber, string teamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    var teamInfo = cvDatabase.TeamAccounts.FirstOrDefault(x => x.Team.Name.Equals(teamName)
                                                                            && x.Status == false);
                    var memberInfo = cvDatabase.TeamMembers.FirstOrDefault(x => x.UserID.Equals(username));
                    var userInfo = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username) && x.Password.Equals(password));
                    var isTeamAdmin = cvDatabase.Teams.FirstOrDefault(x => x.Name.Equals(teamName) && x.AdminName.Equals(username));
                    if (teamInfo!=null && (memberInfo!=null || isTeamAdmin!=null )  && userInfo!=null)
                    {
                        UserLoginHistory newUserLoginHistory = new UserLoginHistory 
                                                                                 {
                                                                                     AutoInc =default(long),
                                                                                     CounterNumber=counterNumber,
                                                                                     StratTime=DateTime.Now,
                                                                                     TeamName=teamName,
                                                                                     UserID=username
                                                                                 };
                        cvDatabase.AddToUserLoginHistories(newUserLoginHistory);
                        teamInfo.Status = true;
                        var userinfo=  cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                        userinfo.Status = false;
                        userinfo.Counternumber = counterNumber;
                        cvDatabase.SaveChanges();
                        List<Int64> teamLoginInfo = new List<Int64>();
                        teamLoginInfo.Add(teamInfo.Minutes);
                        teamLoginInfo.Add(newUserLoginHistory.AutoInc);
                        return teamLoginInfo;
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 1]);
                    }
                }
            }
            catch (Exception error)
            {
                throw new FaultException(error.Message);
            }
        }

        public bool UserMunitieCounter(string username, short counterNumber)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    var userAccount = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username) && x.Minutes > 1);
                    if (userAccount!=null)
                    {
                        userAccount.Minutes--;
                        userAccount.Status = true;
                        userAccount.Counternumber = counterNumber;
                        cvDatabase.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception error)
            {
                throw new FaultException(error.Message);
            }
        }

        public bool TeamMunitieCounter(string teamName,string username, short counterNumber)
        {
            try
            {
                lock (signalEntry)
                {
                    using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                    {
                        var teamAccount = cvDatabase.TeamAccounts.FirstOrDefault(x => x.Name.Equals(teamName) && x.Minutes > 1);
                        var userAccount = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                        if (teamAccount != null && userAccount != null)
                        {
                            teamAccount.Minutes--;
                            teamAccount.Status = true;
                            userAccount.Status = true;
                            userAccount.Counternumber = counterNumber;
                            cvDatabase.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    } 
                   
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 4]);
            }
        }

        public Userinformation AccountDetails(string username)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from userinfoTable in cvDatabase.CustomerInformations
                                where userinfoTable.UserID.Trim().Equals(username)
                                select new Userinformation
                                                         {
                                                             Address = userinfoTable.Address,
                                                             Date = userinfoTable.JoinDate,
                                                             Email = userinfoTable.Email,
                                                             Name = userinfoTable.Name,
                                                             NationalID = userinfoTable.NationalID,
                                                             Phone = userinfoTable.Phone,
                                                             UserImage = userinfoTable.Logo
                                                         }).SingleOrDefault();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool AccountUpdate(string username, Userinformation updateinfo)
        { 
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    var customerInfo = cvDatabase.CustomerInformations.First(x => x.UserID.Equals(username));
                    customerInfo.Address = updateinfo.Address;
                    customerInfo.Email = updateinfo.Email;
                    customerInfo.Logo = updateinfo.UserImage;
                    customerInfo.NationalID = updateinfo.NationalID;
                    customerInfo.Phone = updateinfo.Phone;
                    cvDatabase.SaveChanges();
                    return true;
                }  
                
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0,2]);
            }
        }

        public List<LoginHistory> UserLoginHistoryAll(string username)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from loginHisTable in cvDatabase.UserLoginHistories
                            where loginHisTable.UserID.Equals(username)
                            select new LoginHistory
                                                 {
                                                     CounterNumber = loginHisTable.CounterNumber,
                                                     EntryDate = loginHisTable.StratTime,
                                                     MinutesUse = (loginHisTable.EndTime.Value.Minute - loginHisTable.StratTime.Minute),
                                                     TeamName=loginHisTable.TeamName
                                                 }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<LoginHistory> UserLoginHistoryDate(string username, DateTime date)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var data= (from loginHisTable in cvDatabase.UserLoginHistories
                            where loginHisTable.UserID.Equals(username) 
                            select new LoginHistory
                            {
                                CounterNumber = loginHisTable.CounterNumber,
                                EntryDate = loginHisTable.StratTime,
                                MinutesUse = loginHisTable.EndTime.Value.Minute - loginHisTable.StratTime.Minute,
                                TeamName = loginHisTable.TeamName
                            }).ToList();

                    return data.Where(x => x.EntryDate.Value.Date == date).ToList();
                }
            }
            catch(Exception ex)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<LoginHistory> UserLoginHistoryTwoDate(string username, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var data= (from loginHisTable in cvDatabase.UserLoginHistories
                            where loginHisTable.UserID.Equals(username) && loginHisTable.StratTime.Date <= firstDate && loginHisTable.StratTime.Date >= secondDate 
                            select new LoginHistory
                            {
                                CounterNumber = loginHisTable.CounterNumber,
                                EntryDate = loginHisTable.StratTime,
                                MinutesUse = (loginHisTable.EndTime.Value.Minute - loginHisTable.StratTime.Minute),
                                TeamName = loginHisTable.TeamName
                            }).ToList();
                    return data.Where(x => x.EntryDate.Value.Date <= firstDate && x.EntryDate.Value.Date >= secondDate).ToList();
                }
            }
            catch(Exception ex)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<UserRechareHistory> UserRechargeHistoryAll(string username)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    return (from rechargeHisTable in cvDatabase.UserRechargeHistories
                            where rechargeHisTable.UserID.Equals(username)
                            select new UserRechareHistory
                                                       {
                                                           Amount = rechargeHisTable.bill,
                                                           EmployID=rechargeHisTable.EmployeeID,
                                                           EntryDate=rechargeHisTable.DateWithTime,
                                                           Munities=rechargeHisTable.Minutes
                                                       }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<UserRechareHistory> UserRechargeHistoryDate(string username, DateTime date)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from rechargeHisTable in cvDatabase.UserRechargeHistories
                            where rechargeHisTable.UserID.Equals(username) && rechargeHisTable.DateWithTime == date
                            select new UserRechareHistory
                            {
                                Amount = rechargeHisTable.bill,
                                EmployID = rechargeHisTable.EmployeeID,
                                EntryDate = rechargeHisTable.DateWithTime,
                                Munities = rechargeHisTable.Minutes
                            }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<UserRechareHistory> UserRechargeHistoryTwoDate(string username, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from rechargeHisTable in cvDatabase.UserRechargeHistories
                            where rechargeHisTable.UserID.Equals(username) && rechargeHisTable.DateWithTime<=firstDate && rechargeHisTable.DateWithTime >=secondDate
                            select new UserRechareHistory
                            {
                                Amount = rechargeHisTable.bill,
                                EmployID = rechargeHisTable.EmployeeID,
                                EntryDate = rechargeHisTable.DateWithTime,
                                Munities = rechargeHisTable.Minutes
                            }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<AllUserAndTeam> GetAllUsers()
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from alluserInfo in cvDatabase.CustomerInformations
                            select new AllUserAndTeam
                                                  {
                                                      Name =alluserInfo.UserID.Trim()
                                                  }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool TeamNameChecker(string teamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    if (cvDatabase.Teams.SingleOrDefault(x => x.Name.Trim().Equals(teamName)) == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0,2]);
            }
        }

        public bool AddNewTeam(TeamInfo teamInfo, List<AllUserAndTeam> members)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    cvDatabase.AddToTeams(new Team
                                                {
                                                    AdminName = teamInfo.TeamAdmin,
                                                    JoinDate = teamInfo.EntryDate,
                                                    Logo = teamInfo.TeamImage,
                                                    Name = teamInfo.TeamD,
                                                    TeamAccount = new TeamAccount
                                                                    {
                                                                        Minutes = 0,
                                                                        Name = teamInfo.TeamD,
                                                                        Status = false,
                                                                        EntryDate = DateTime.Today
                                                                    }
                                                });
                    foreach (AllUserAndTeam teamMember in members)
                    {
                        cvDatabase.AddToTeamMembers(new TeamMember
                                                                {
                                                                    AutoInc = default(long),
                                                                    UserID = teamMember.Name,
                                                                    Name = teamInfo.TeamD

                                                                });
                    }
                    cvDatabase.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool DeleteMember(string teamName, AllUserAndTeam member)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    cvDatabase.TeamMembers.DeleteObject(cvDatabase.TeamMembers.First(x => x.UserID.Equals(member.Name) && x.Name.Equals(teamName)));
                    cvDatabase.SaveChanges();
                    return true;
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public counterSetting CounterSettings()
        {
            try
            {
                  string counterConfigXmlpath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cafeteria_Vernier"),  "countConfig.xml");
                  if (!File.Exists(counterConfigXmlpath))
                  {
                      var counterConfig = XDocument.Load(counterConfigXmlpath);
                      counterSetting countSetting = (from countConfig in counterConfig.Descendants("MunitiesCount") select new counterSetting { seconds = (int)countConfig.Attribute("second") }).SingleOrDefault();
                      return countSetting;
                  }
                  else
                  {
                      return new counterSetting { seconds = 60 };
                  }
            }
            catch (Exception error)
            {
                throw new FaultException(error.Message);
            }
        }

        public byte[] GetUserImage(string username)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return cvDatabase.CustomerInformations.First(x => x.UserID.Equals(username)).Logo;
                }
            }
            catch
            {    
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0,2]);
            }
        }

        public List<AllUserAndTeam> GetTeamName(string adminName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from teamTable in cvDatabase.Teams
                            where teamTable.AdminName.Equals(adminName)
                            select new AllUserAndTeam
                                                     {
                                                         Name=teamTable.Name.Trim()
                                                     }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool UpdateTeam(string teamName, List<AllUserAndTeam> teamMembers)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                    foreach (var existingMember in cvDatabase.TeamMembers.Where(x=>x.Name.Equals(teamName)))
                    {
                        cvDatabase.TeamMembers.DeleteObject(existingMember);
                    }
                    foreach (AllUserAndTeam teamMember in teamMembers)
                    {
                        cvDatabase.AddToTeamMembers(new TeamMember { Name = teamName, UserID = teamMember.Name });
                    }
                    cvDatabase.SaveChanges();
                    return true;
                }
            }
            catch (Exception error)
            {
                throw new FaultException(error.Message);
            }
        }

        public List<UserRechareHistory> TeamRechargeHistoryAll(string TeamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase= new Cafeteria_Vernier_dbEntities())
                {
                   return (from rechargeHisTable in cvDatabase.TeamRechargeHistories where rechargeHisTable.Name.Equals(TeamName) select new UserRechareHistory 
                                                                                                                                                            {
                                                                                                                                                                Amount=rechargeHisTable.bill,
                                                                                                                                                                EmployID=rechargeHisTable.EmployeeID,
                                                                                                                                                                EntryDate=rechargeHisTable.DateWithTime,
                                                                                                                                                                Munities=rechargeHisTable.Minutes
                                                                                                                                                           }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<UserRechareHistory> TeamRechargeHistoryDate(string TeamName, DateTime date)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from rechargeHisTable in cvDatabase.TeamRechargeHistories
                            where rechargeHisTable.Name.Equals(TeamName) && rechargeHisTable.DateWithTime ==date
                            select new UserRechareHistory
                            {
                                Amount = rechargeHisTable.bill,
                                EmployID = rechargeHisTable.EmployeeID,
                                EntryDate = rechargeHisTable.DateWithTime,
                                Munities = rechargeHisTable.Minutes
                            }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<UserRechareHistory> TeamRechargeHistoryTwoDate(string TeamName, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from rechargeHisTable in cvDatabase.TeamRechargeHistories
                            where rechargeHisTable.Name.Equals(TeamName) && rechargeHisTable.DateWithTime<=firstDate && rechargeHisTable.DateWithTime>=secondDate
                            select new UserRechareHistory
                            {
                                Amount = rechargeHisTable.bill,
                                EmployID = rechargeHisTable.EmployeeID,
                                EntryDate = rechargeHisTable.DateWithTime,
                                Munities = rechargeHisTable.Minutes
                            }).ToList();
                }
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool UserLogout(string username, Int64 loginHistoryID)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var userinfo = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                    if (userinfo!=null)
                    {
                        userinfo.Status = false;
                        userinfo.Counternumber = 0;
                        cvDatabase.UserLoginHistories.First(x => x.AutoInc == loginHistoryID).EndTime = DateTime.Now;
                        cvDatabase.SaveChanges();
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 3]);
                    }
                }
                return true;
            }
            catch
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool TeamLogout(string username, string teamName, Int64 loginHistoryID)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var userInfo = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                    var teamInfo = cvDatabase.TeamAccounts.FirstOrDefault(x => x.Name.Equals(teamName));
                    if (userInfo !=null && teamInfo!=null)
                    {
                        userInfo.Status = false;
                        userInfo.Counternumber = 0;
                        teamInfo.Status = false;
                        cvDatabase.UserLoginHistories.First(x => x.AutoInc == loginHistoryID).EndTime = DateTime.Now;
                        cvDatabase.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 3]);
                    }
                }
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public List<AllUserAndTeam> GetTeamMember(string teamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return (from teamMember in cvDatabase.TeamMembers where teamMember.Name.Equals(teamName) select new AllUserAndTeam 
                                                                                                                                    {
                                                                                                                                        Name=teamMember.UserID.Trim()
                                                                                                                                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 4]);
            }
        }

        public int GetUserBalance(string username)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var userInfo = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                    if (userInfo!=null)
                    {
                        return userInfo.Minutes;
                    } 
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
                    }
                }
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public int GetTeamBalance(string teamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var teamInfo = cvDatabase.TeamAccounts.FirstOrDefault(x => x.Name.Equals(teamName));
                    if (teamInfo!=null)
                    {
                        return teamInfo.Minutes;
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
                    }
                }
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public bool PasswordChange(string username, string newPassword)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    var customerInfo = cvDatabase.CustomerAccounts.FirstOrDefault(x => x.UserID.Equals(username));
                    if (customerInfo!=null)
                    {
                        customerInfo.Password = newPassword;
                        cvDatabase.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
                    }
                }
            }
            catch (Exception)
            {
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public byte[] GetTeamLogo(string teamName)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    return cvDatabase.Teams.First(x => x.Name.Equals(teamName)).Logo;
                }
            }
            catch (Exception)
            {
                
                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }

        public void UpdateTeamLogo(string teamName, byte[] teamLogo)
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    cvDatabase.Teams.First(x => x.Name.Equals(teamName)).Logo = teamLogo;
                }
            }
            catch (Exception)
            {

                throw new FaultException(ServiceVariables.ERROR_MESSAGES[0, 2]);
            }
        }


    }
}

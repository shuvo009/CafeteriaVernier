using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.serverSideService.Class
{
   public class ServiceVariables
    {
     public static string[,] ERROR_MESSAGES = new string[,]
       { 
       {"Wrong username or password or No enough Minutes","Wrong username OR password OR Team \nOR You are not a member \nOR No enough Minutes","Access Denied !","Logout process is not success.","Account is using another counter."}
       //                    00                                                                       01                                              02                          03                        04                         05                          06
       };
    }
}

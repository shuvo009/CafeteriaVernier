using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using Procesta.ClientSideService.Class;
using Procesta.ClientSideService.Class.Data;
using Procesta.ClientSideService.Class.Methods;
using Procesta.ClientSideService.Class.Property;
using Procesta.RegeditClasss;

namespace Procesta.ClientSideService
{
    public class ClientSideService : IClientSideService
    {

        public bool CounterConfigerSelective(List<ConfigerStatus> configerStatus)
        {
            try
            {
                RegeditWriteRead writeToReg = new RegeditWriteRead();
                var tempRegistryInfo = from allRegistryData in new RegistryData().registryDataCollection from confiStatus in configerStatus where allRegistryData.Name.Equals(confiStatus.Name) select new { allRegistryData, confiStatus };
                foreach (var registryEntry in tempRegistryInfo)
                {
                    if (registryEntry.confiStatus.Status)
                    {
                        writeToReg.RegistryWriter(registryEntry.allRegistryData.Name, registryEntry.allRegistryData.keyName, registryEntry.allRegistryData.enableValue, registryEntry.allRegistryData.valueKind);
                    }
                    else
                    {
                        writeToReg.RegistryWriter(registryEntry.allRegistryData.Name, registryEntry.allRegistryData.keyName, registryEntry.allRegistryData.disableValue, registryEntry.allRegistryData.valueKind);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw new FaultException(ClientVariables.ERROR_MESSAGES[0, 0]);
            }
            finally
            {
                this.RestartCounter();
            }
        }

        public List<ConfigerStatus> GetCurrentConfigration()
        {
            try
            {
                return new LoadConfigration().LoadCounterConfiger(); ;
            }
            catch (Exception)
            {
                throw new FaultException(ClientVariables.ERROR_MESSAGES[0,0]);
            }
        }

        public bool ShutdownCounter()
        {
            try
            {
                Process.Start("shutdown","/s /t 0");
                return true;
            }
            catch (Exception)
            { 
               throw new FaultException(ClientVariables.ERROR_MESSAGES[0,0]);
            }
        }

        public bool RestartCounter()
        {
           try
            {
                Process.Start("shutdown", "/r /t 0");
                return true;
            }
            catch (Exception)
            { 
               throw new FaultException(ClientVariables.ERROR_MESSAGES[0,0]);
            }
        }

        public bool LogOff()
        {
           try
            {
                Process.Start("shutdown", "/l /t 0");
                return true;
            }
            catch (Exception)
            { 
               throw new FaultException(ClientVariables.ERROR_MESSAGES[0,0]);
            }
        }

        public bool ClientOff()
        {
            try
            {
                foreach (Process CvClientProcess in Process.GetProcessesByName("CVClient"))
                {
                    CvClientProcess.Kill();
                    CvClientProcess.WaitForExit(); 
                    return true;
                }
                return false;
               
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool ClientStart()
        {
            try
            {
                Process cvClientProcess = new Process();
                cvClientProcess.StartInfo.UseShellExecute = true;
                cvClientProcess.StartInfo.CreateNoWindow = false;
                cvClientProcess.StartInfo.FileName=System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CVClient.exe");
                cvClientProcess.Start();
                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}

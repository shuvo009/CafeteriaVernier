using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Procesta.ClientSideService.Class.Property;
namespace Procesta.ClientSideService
{
    [ServiceContract]
    public interface IClientSideService
    {
        /// <summary>
        /// Counter Configuration With Selective  Futures
        /// </summary>
        /// <param name="configerStatus">Type of ConfigerStatus</param>
        /// <returns>True for Success</returns>
        [OperationContract]
        bool CounterConfigerSelective(List<ConfigerStatus> configerStatus);
        /// <summary>
        /// Get Current Configurations of a counter 
        /// </summary>
        /// <returns>type of ConfigerStatus</returns>
        [OperationContract]
        List<ConfigerStatus> GetCurrentConfigration();
        /// <summary>
        /// To shutdown A Counter
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool ShutdownCounter();
        /// <summary>
        /// To Restart A Counter
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool RestartCounter();
        /// <summary>
        /// To logoff A Counter
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool LogOff();
        [OperationContract]
        bool ClientOff();
        [OperationContract]
        bool ClientStart();
    }
}

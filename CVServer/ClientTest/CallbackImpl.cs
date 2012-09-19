using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUse;

namespace ClientTest
{
    class CallbackImpl : IDataOutputCallback
    {
        #region IDataOutputCallback Members

        public void SendDataPacket()
        {
            Console.WriteLine("Server sent: {0}");
        }

        #endregion
    }
}

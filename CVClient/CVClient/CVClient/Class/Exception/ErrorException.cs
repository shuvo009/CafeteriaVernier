using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.CVClient.Class.Exceptions
{
    class ErrorException : Exception
    {
         public ErrorException()
        {
            
        }
        public ErrorException(string message)
            : base(message)
        {
            
        }
        public ErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected ErrorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}

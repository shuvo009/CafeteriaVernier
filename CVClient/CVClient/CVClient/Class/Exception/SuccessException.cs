using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.CVClient.Class.Exceptions
{
    class SuccessException : Exception
    {
         public SuccessException()
        {
            
        }
        public SuccessException(string message)
            : base(message)
        {
            
        }
        public SuccessException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected SuccessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}

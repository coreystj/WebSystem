using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Exceptions
{

    [Serializable]
    public class ServerException : Exception
    {
        public ServerException() { }
        public ServerException(string message) : base(message) { }
        public ServerException(string message, Exception inner) : base(message, inner) { }
        protected ServerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

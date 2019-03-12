using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Exceptions
{

    [Serializable]
    public class UnityException : Exception
    {
        public UnityException() { }
        public UnityException(string message) : base(message) { }
        public UnityException(string message, Exception inner) : base(message, inner) { }
        protected UnityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

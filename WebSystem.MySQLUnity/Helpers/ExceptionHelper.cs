using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSystem.MySQLUnity.Exceptions;

namespace WebSystem.MySQLUnity.Helpers
{
    public static class ExceptionHelper
    {
        public static void GetException(this Dictionary<string, object> dict)
        {
            var isError = dict.SafeGet("IsError", false);
            if (isError)
            {
                var rawException = dict.SafeGet("Exception", new Dictionary<string, object>());
                var className = rawException.SafeGet("ClassName", string.Empty);
                var message = rawException.SafeGet("Message", string.Empty);
                var stackTraceString = rawException.SafeGet("StackTraceString", string.Empty);

                throw new ServerException(className + ": " + message + "\n\nSTACKTRACE: " + stackTraceString);
            }
        }
    }
}

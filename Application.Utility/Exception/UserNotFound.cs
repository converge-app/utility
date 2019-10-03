using System;
using System.Runtime.Serialization;

namespace Application.Utility.Exception
{
    [Serializable]
    public class UserNotFound : System.Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UserNotFound()
        {
        }

        public UserNotFound(string message) : base(message)
        {
        }

        public UserNotFound(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected UserNotFound(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
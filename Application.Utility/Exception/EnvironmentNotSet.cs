using System;
using System.Runtime.Serialization;

namespace Application.Utility.Exception
{
    [Serializable]
    public class EnvironmentNotSet : System.Exception
    {
        public EnvironmentNotSet()
        {
        }

        public EnvironmentNotSet(string message) : base(message)
        {
        }

        public EnvironmentNotSet(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected EnvironmentNotSet(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
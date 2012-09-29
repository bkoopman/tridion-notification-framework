using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TridionCommunity.NotificationFramework
{

    [Serializable]
    public class NotificationFailedException : Exception
    {
        public NotificationFailedException() { }
        public NotificationFailedException(string message) : base(message) { }
        public NotificationFailedException(string message, Exception inner) : base(message, inner) { }
        protected NotificationFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}


using System;

namespace MasDev.Common.Push
{
    public class PushFailedException : PushException
    {
        public Exception Error { get; private set; }

        public PushFailedException(Exception error)
        {
            Error = error;
        }
    }
}

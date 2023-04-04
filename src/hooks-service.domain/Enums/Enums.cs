using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHooks.Domain.Enums
{
    public class Enums
    {
        public struct EventStatus
        {
            public const string PENDING = "PENDING";
            public const string DONE = "DONE";
            public const string REMOVED = "REMOVED";
        }

        public struct EventResultTypes
        {
            public const string OK = "OK";
            public const string ERROR = "ERROR";
        }

        public struct Errors
        {
            public const string FATAL_ERROR = "FATAL ERROR";
        }
    }
}

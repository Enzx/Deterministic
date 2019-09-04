using System;
using System.Diagnostics;

namespace Deterministic.Logs
{
    public static class Log
    {
        [Conditional("ENABLE_EXCEPTIONS")]
        public static void ThrowException(string message)
        {
            throw new Exception(message);
        }

        public static void LogException(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
using System;
using System.Diagnostics;

namespace TheQDeviceConnect.Core.Helpers
{
    public static class DebugHelper
    {
        public static void Warning(object classInstance, object message)
        {
            Debug.WriteLine($"WARNING AT: {classInstance.ToString()} : MESSAGE : {message}");
            Debug.WriteLine($"WARNING AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
        }

        //Overloading
        public static void Warning(object classInstance, object message, object methodName)
        {
            Debug.WriteLine($"ERROR AT: {classInstance.ToString()} : MESSAGE : {message}");
            Debug.WriteLine($"WARNING AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
            TraceCall(methodName);
        }

        public static void Error(object classInstance, object message)
        {
            Debug.WriteLine($"INFO AT: {classInstance.ToString()} : MESSAGE : {message}");
            Debug.WriteLine($"ERROR AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
        }

        //Overloading
        public static void Error(object classInstance, object message, object methodName)
        {
            Debug.WriteLine($"ERROR AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
            TraceCall(methodName);
        }

        public static void Info(object classInstance, object message)
        {
            Debug.WriteLine($"INFO AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
        }

        //Overloading
        public static void Info(object classInstance, object message, object methodName)
        {
            Trace.WriteLine($"INFO AT: {classInstance.ToString()} : MESSAGE : {message.ToString()}");
            TraceCall(methodName);
        }

        public static void TraceCall(object methodName)
        {
            Debug.WriteLine($"METHOD: {methodName.ToString()} CALLED");

        }

    }
}

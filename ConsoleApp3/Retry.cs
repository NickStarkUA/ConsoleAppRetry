using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp3
{
    public static class Retry
    {
        public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Do<object>(() => {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }
            exceptions.Add(new Exception(string.Format("Method call failed after {0} retries with {1} second intervals.", retryCount, retryInterval)));
            throw new AggregateException(exceptions);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publications.ConsoleTests
{
    internal static class Synchronization
    {
        private static readonly object __SyncRoot = new object();

        public static void Run()
        {
            //lock (__SyncRoot)
            //{
            //    // критическая секция
            //}

            Monitor.Enter(__SyncRoot);
            try
            {
                lock (__SyncRoot)
                {
                    // критическая секция
                }
            }
            finally
            {
                Monitor.Exit(__SyncRoot);
            }
        }
    }
}

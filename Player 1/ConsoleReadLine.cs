using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Player_1
{
    class ConsoleReadLine
    {
        private static string inputLast;
        private static Thread inputThread = new Thread(inputThreadAction) { IsBackground = true };
        private static AutoResetEvent inputGet = new AutoResetEvent(false);
        private static AutoResetEvent inputGot = new AutoResetEvent(false);

        static ConsoleReadLine()
        {
            inputThread.Start();
        }

        private static void inputThreadAction()
        {
            while (true)
            {
                inputGet.WaitOne();
                inputLast = Console.ReadLine();
                inputGot.Set();
            }
        }

        public static string ReadLine(int timeout = Timeout.Infinite)
        {
            if (timeout == Timeout.Infinite)
            {
                return Console.ReadLine();
            }
            else
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                while (stopwatch.ElapsedMilliseconds < timeout) {
                    if (Console.KeyAvailable)
                    {
                        inputGet.Set();
                        inputGot.WaitOne();
                        return inputLast;
                    }
                }
                return "0";
            }
        }
    }
}

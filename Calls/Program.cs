using System.Threading;
using System;
using Calls.Lib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calls
{
    class Program
    {
        static void Main(string[] args)
        {
            var callCenter = new CallCenter(
                new List<Agent>()
                {
                    new Agent("a1"),
                    new Agent("a2"),
                    new Agent("a3"),
                },
                2
            );

            callCenter.CallStarted += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' started by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            callCenter.CallFinished += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' finished by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            callCenter.IncomingCallApeared += c =>
            {
                Console.WriteLine("Incoming call apeared. Number of calls: {0}", c.NumberOfCalls);
            };

            callCenter.Start();

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.N)
                {
                    Console.WriteLine("Incoming call added by user.");
                    callCenter.AddIncomingCall();
                }
                Thread.Sleep(100);
            }
        }
    }
}

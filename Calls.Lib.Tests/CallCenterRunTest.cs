using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace Calls.Lib.Tests
{
    public class CallCenterRunTest
    {
        [Fact]
        public void RunApp()
        {
            var c = new CallCenter(
                new List<Agent>()
                {
                    new Agent("a1"),
                    new Agent("a2"),
                    new Agent("a3"),
                },
                2
            );

            c.IncomingCallApeared += c =>
            {
                Console.WriteLine("Incoming call apeared. Number of calls: {0}", c.NumberOfCalls);
            };

            c.CallStarted += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' started by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            c.CallFinished += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' finished by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            c.Start();

            var task = Task.Run(() =>
            {
                Thread.Sleep(1000 * 60 * 60 * 60);
            });

            Task.WaitAll(task);
        }

        [Fact]
        public void RunAppWithIncomingCalls()
        {
            var c = new CallCenter(
                new List<Agent>()
                {
                    new Agent("a1"),
                    new Agent("a2"),
                    new Agent("a3"),
                },
                2
            );

            c.IncomingCallApeared += c =>
            {
                Console.WriteLine("Incoming call apeared. Number of calls: {0}", c.NumberOfCalls);
            };

            c.CallStarted += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' started by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            c.CallFinished += c =>
            {
                Console.WriteLine("Call '{0}' with duration '{1}s' finished by '{2}'", c.Id, c.DurationInSec, c.AgentName);
            };

            c.Start();

            var addNewCallTask = Task.Run(() =>
            {
                while (true)
                {
                    c.AddIncomingCall();
                    Thread.Sleep(3000);
                }
            });

            var task = Task.Run(() =>
            {
                Thread.Sleep(1000 * 60 * 60 * 60);
            });

            Task.WaitAll(task);
        }
    }
}
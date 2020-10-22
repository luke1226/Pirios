using System.Collections.Generic;
using Xunit;

namespace Calls.Lib.Tests
{
    public class App
    {
        [Fact]
        public void RunApp()
        {
            var c = new Central(
                new List<Agent>()
                {
                    new Agent("a1"),
                    new Agent("a2"),
                    new Agent("a3"),
                }
            );
            c.AddIncomingCall();
            c.AddIncomingCall();

            c.StartNewCall();
        }
    }
}
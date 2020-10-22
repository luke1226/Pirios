using System.Threading;
using System.Threading.Tasks;
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
                },
                10
            );

            c.StartNewCall();

            var task = Task.Run(() =>
            {

                Thread.Sleep(1 * 60 * 60 * 60);
            });

            Task.WaitAll(task);
        }
    }
}
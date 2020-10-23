using System.Threading;
using System;
using Calls.Lib;

namespace Calls.Lib
{
    class Call
    {
        public int Id { get; }
        public int DurationInSec { get; }
        public Agent Agent { get; }

        public event Action<Call> CallFinished;

        public Call(int id, int durationInSec, Agent agent)
        {
            Id = id;
            DurationInSec = durationInSec;
            Agent = agent;
        }

        public void Start()
        {
            Thread.Sleep(DurationInSec * 1000);
            CallFinished(this);
        }
    }
}

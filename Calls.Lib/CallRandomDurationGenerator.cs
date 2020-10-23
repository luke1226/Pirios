using System;
namespace Calls.Lib
{
    class CallRandomDurationGenerator
    {
        private Random _rand = new Random(DateTime.Now.Millisecond);

        public int GenerateDurationInSec()
        {
            return (_rand.Next() % 10) + 5;
        }
    }

    class TimeoutBetweenCallsRandomDurationGenerator
    {
        private Random _rand = new Random(DateTime.Now.Millisecond);

        public int GenerateDurationInSec()
        {
            return (_rand.Next() % 10) + 1;
        }
    }
}
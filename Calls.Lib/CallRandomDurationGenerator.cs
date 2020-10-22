using System;
namespace Calls.Lib
{
    public class CallRandomDurationGenerator
    {
        private Random _rand = new Random(DateTime.Now.Millisecond);

        public int GenerateDurationInSec()
        {
            return (_rand.Next() % 10) + 5;
        }
    }
}
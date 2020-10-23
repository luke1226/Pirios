namespace Calls.Lib
{
    public class CallEventArgs
    {
        public int Id { get; set; }
        public string AgentName { get; set; }
        public int DurationInSec { get; set; }
    }

    public class IncomingCallEventArgs
    {
        public int NumberOfCalls { get; set; }
    }
}
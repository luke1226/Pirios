using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Calls.Lib
{
    public class Central
    {
        private ConcurrentStack<Agent> _agents;
        private ConcurrentStack<IncomingCall> _incomingCallList = new ConcurrentStack<IncomingCall>();
        private NewIncomingCallGenerator _newIncomingCallGenerator = new NewIncomingCallGenerator();
        private CallRandomDurationGenerator _callRandomDurationGenerator = new CallRandomDurationGenerator();
        private object _lock = new object();

        public Central(IEnumerable<Agent> agents)
        {
            _agents = new ConcurrentStack<Agent>(agents);
        }

        public void StartNewCall()
        {
            Agent agent;
            IncomingCall iCall;

            lock (_lock)
            {
                if (_incomingCallList.Count == 0)
                    return;

                if (_agents.Count == 0)
                    return;

                _agents.TryPop(out agent);
                _incomingCallList.TryPop(out iCall);
            }

            var durationInSec = _callRandomDurationGenerator.GenerateDurationInSec();
            var call = new Call(iCall.Id, durationInSec, agent);
            call.CallFinished += OnCallFinished;
        }

        private void OnCallFinished(Call call)
        {
            call.CallFinished -= OnCallFinished;
        }

        public void AddIncomingCall()
        {
            lock (_lock)
            {
                var iCall = _newIncomingCallGenerator.GenerateNew();
                _incomingCallList.Push(iCall);
            }

            StartNewCall();
        }
    }


}
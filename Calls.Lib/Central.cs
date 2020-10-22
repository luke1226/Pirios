using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Calls.Lib
{
    public class Central
    {
        private ConcurrentStack<Agent> _agents;
        private ConcurrentStack<IncomingCall> _incomingCallList;
        private NewIncomingCallGenerator _newIncomingCallGenerator = new NewIncomingCallGenerator();
        private CallRandomDurationGenerator _callRandomDurationGenerator = new CallRandomDurationGenerator();
        private object _lock = new object();

        public Central(IEnumerable<Agent> agents, int incomingCalls)
        {
            _agents = new ConcurrentStack<Agent>(agents);
            _incomingCallList = new ConcurrentStack<IncomingCall>(
                GenerateInitIncomingList(incomingCalls));
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

            call.Start();
        }

        private void OnCallFinished(Call call)
        {
            call.CallFinished -= OnCallFinished;

            StartNewCall();
        }

        private List<IncomingCall> GenerateInitIncomingList(int count)
        {
            var result = new List<IncomingCall>();
            for (int i = 0; i < count; i++)
            {
                var iCall = _newIncomingCallGenerator.GenerateNew();
                result.Add(iCall);
            }

            return result;
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
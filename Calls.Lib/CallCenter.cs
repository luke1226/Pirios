using System.Threading;
using System.Collections.Generic;
using System;
using Calls.Lib;
using System.Threading.Tasks;

namespace Calls.Lib
{
    public class CallCenter
    {
        private Queue<Agent> _agents;
        private Queue<IncomingCall> _incomingCallList;
        private NewIncomingCallGenerator _newIncomingCallGenerator = new NewIncomingCallGenerator();
        private CallRandomDurationGenerator _callRandomDurationGenerator = new CallRandomDurationGenerator();
        private TimeoutBetweenCallsRandomDurationGenerator _timeoutBetweenCallsRandomDurationGenerator = new TimeoutBetweenCallsRandomDurationGenerator();
        private object _lock = new object();



        public CallCenter(IEnumerable<Agent> agents, int initIncomingCalls)
        {
            _agents = new Queue<Agent>(agents);
            _incomingCallList = new Queue<IncomingCall>(
                GenerateInitIncomingList(initIncomingCalls));
        }



        public void Start()
        {
            StartNewCall();

            Task.Run(() =>
            {
                while (true)
                {
                    var timeout = _timeoutBetweenCallsRandomDurationGenerator.GenerateDurationInSec();
                    Thread.Sleep(timeout * 1000);

                    AddIncomingCall();
                }
            });
        }

        public void AddIncomingCall()
        {
            int numberOfCalls;

            lock (_lock)
            {
                var iCall = _newIncomingCallGenerator.GenerateNew();
                _incomingCallList.Enqueue(iCall);

                numberOfCalls = _incomingCallList.Count;
            }

            InformIncomingCallApeared(numberOfCalls);

            StartNewCall();
        }

        public void AddNewAgent(string name)
        {
            var newAgent = new Agent(name);

            lock (_lock)
            {
                _agents.Enqueue(newAgent);
            }

            StartNewCall();
        }



        private void StartNewCall()
        {
            Agent agent;
            IncomingCall iCall;

            lock (_lock)
            {
                if (_incomingCallList.Count == 0)
                    return;

                if (_agents.Count == 0)
                    return;

                agent = _agents.Dequeue();
                iCall = _incomingCallList.Dequeue();
            }

            var durationInSec = _callRandomDurationGenerator.GenerateDurationInSec();
            var call = new Call(iCall.Id, durationInSec, agent);
            call.CallFinished += OnCallFinished;

            Task.Run(() => call.Start());
            InformCallStarted(call);
        }

        private void OnCallFinished(Call call)
        {
            InformCallFinished(call);

            call.CallFinished -= OnCallFinished;
            _agents.Enqueue(call.Agent);

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



        public event Action<IncomingCallEventArgs> IncomingCallApeared;
        public event Action<CallEventArgs> CallStarted;
        public event Action<CallEventArgs> CallFinished;

        private void InformIncomingCallApeared(int numberOfCalls)
        {
            if (IncomingCallApeared == null)
                return;

            IncomingCallApeared(new IncomingCallEventArgs()
            {
                NumberOfCalls = numberOfCalls
            });
        }
        private void InformCallStarted(Call call)
        {
            if (CallStarted == null)
                return;

            CallStarted(new CallEventArgs()
            {
                Id = call.Id,
                DurationInSec = call.DurationInSec,
                AgentName = call.Agent.Name
            });
        }

        private void InformCallFinished(Call call)
        {
            if (CallFinished == null)
                return;

            CallFinished(new CallEventArgs()
            {
                Id = call.Id,
                DurationInSec = call.DurationInSec,
                AgentName = call.Agent.Name
            });
        }
    }
}
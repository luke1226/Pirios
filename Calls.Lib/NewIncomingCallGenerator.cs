using System;
namespace Calls.Lib
{
    class NewIncomingCallGenerator
    {
        private int _currentId = 1;
        private object _lockObj = new object();

        public IncomingCall GenerateNew()
        {
            int id;
            lock (_lockObj)
            {
                id = _currentId;
                _currentId++;
            }

            var call = new IncomingCall(id);
            return call;
        }
    }
}
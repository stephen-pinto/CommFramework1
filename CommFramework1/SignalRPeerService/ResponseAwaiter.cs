using System.Collections.Concurrent;

namespace SignalRPeerService
{
    internal class ResponseAwaiter
    {
        public ConcurrentDictionary<string, MessageSigr> _reponseStore;
        public ConcurrentDictionary<string, ManualResetEventSlim> _awaitingResponses;

        public ResponseAwaiter()
        {
            _reponseStore = new ConcurrentDictionary<string, MessageSigr>();
            _awaitingResponses = new ConcurrentDictionary<string, ManualResetEventSlim>();
        }

        public MessageSigr AwaitResponse(string id, ManualResetEventSlim resetEventSlim, int timeout = 5000)
        {
            //Register the await event
            _awaitingResponses.TryAdd(id, resetEventSlim);

            MessageSigr? response = null;
            //Wait for the response
            if (resetEventSlim.Wait(timeout))
            {
                //Remove the event from the list
                _reponseStore.TryRemove(id, out response);
            }

            _awaitingResponses.TryRemove(new KeyValuePair<string, ManualResetEventSlim>(id, resetEventSlim));
            return response!;
        }

        public async void SaveResponse(string id, MessageSigr response)
        {
            await Task.Factory.StartNew(() =>
            {
                _reponseStore.TryAdd(id, response);

                if (_awaitingResponses.ContainsKey(id))
                    _awaitingResponses[id].Set();
            });
        }
    }
}

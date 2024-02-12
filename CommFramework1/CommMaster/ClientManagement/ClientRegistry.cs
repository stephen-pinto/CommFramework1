using System.Collections;

namespace CommMaster.ClientManagement
{
    public interface IClientRegistry : IDictionary<string, Peer>
    {
    }

    public class ClientRegistry : IClientRegistry
    {
        private Dictionary<string, Peer> _registry;

        public ClientRegistry()
        {
            _registry = new Dictionary<string, Peer>();
        }

        public Peer this[string key]
        {
            get
            {
                return _registry[key];
            }
            set
            {
                _registry[key] = value;
            }
        }

        public ICollection<string> Keys => _registry.Keys;

        public ICollection<Peer> Values => _registry.Values;

        public int Count => _registry.Count;

        public bool IsReadOnly => false;

        public void Add(string key, Peer value)
        {
            _registry.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _registry.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _registry.Remove(key);
        }

        public bool TryGetValue(string key, out Peer value)
        {
            return _registry.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Peer> item)
        {
            _registry.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _registry.Clear();
        }

        public bool Contains(KeyValuePair<string, Peer> item)
        {
            return _registry.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Peer>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
            //_registry.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Peer> item)
        {
            return _registry.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, Peer>> GetEnumerator()
        {
            return _registry.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _registry.GetEnumerator();
        }
    }
}

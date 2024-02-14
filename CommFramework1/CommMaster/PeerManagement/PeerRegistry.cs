using System.Collections;

namespace CommMaster.PeerManagement
{
    public interface IPeerRegistry : IDictionary<string, PeerRegistryEntry>
    {}

    public class PeerRegistry : IPeerRegistry
    {
        private Dictionary<string, PeerRegistryEntry> _registry;

        public PeerRegistry()
        {
            _registry = new Dictionary<string, PeerRegistryEntry>();
        }

        public PeerRegistryEntry this[string key]
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

        public ICollection<PeerRegistryEntry> Values => _registry.Values;

        public int Count => _registry.Count;

        public bool IsReadOnly => false;

        public void Add(string key, PeerRegistryEntry value)
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

        public bool TryGetValue(string key, out PeerRegistryEntry value)
        {
            return _registry.TryGetValue(key, out value!);
        }

        public void Add(KeyValuePair<string, PeerRegistryEntry> item)
        {
            _registry.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _registry.Clear();
        }

        public bool Contains(KeyValuePair<string, PeerRegistryEntry> item)
        {
            return _registry.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, PeerRegistryEntry>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
            //_registry.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, PeerRegistryEntry> item)
        {
            return _registry.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, PeerRegistryEntry>> GetEnumerator()
        {
            return _registry.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _registry.GetEnumerator();
        }
    }
}

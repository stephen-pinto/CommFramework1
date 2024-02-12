using System.Collections;

namespace CommMaster.ClientManagement
{
    public interface IClientRegistry : IDictionary<string, Client>
    {
    }

    public class ClientRegistry : IClientRegistry
    {
        private Dictionary<string, Client> _registry;

        public ClientRegistry()
        {
            _registry = new Dictionary<string, Client>();
        }

        public Client this[string key]
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

        public ICollection<Client> Values => _registry.Values;

        public int Count => _registry.Count;

        public bool IsReadOnly => false;

        public void Add(string key, Client value)
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

        public bool TryGetValue(string key, out Client value)
        {
            return _registry.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Client> item)
        {
            _registry.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _registry.Clear();
        }

        public bool Contains(KeyValuePair<string, Client> item)
        {
            return _registry.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Client>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
            //_registry.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Client> item)
        {
            return _registry.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, Client>> GetEnumerator()
        {
            return _registry.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _registry.GetEnumerator();
        }
    }
}

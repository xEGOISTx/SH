using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public abstract class DeviceBaseList<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
	{
		protected readonly Dictionary<TKey, TValue> _devices = new Dictionary<TKey, TValue>();

		public TValue this[TKey key] => _devices[key];

		public IEnumerable<TKey> Keys => _devices.Keys;

		public IEnumerable<TValue> Values => _devices.Values;

		public int Count => _devices.Count;

		public bool ContainsKey(TKey key)
		{
			return _devices.ContainsKey(key);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return _devices.GetEnumerator();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _devices.TryGetValue(key, out value);
		}

		public IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}

	}
}

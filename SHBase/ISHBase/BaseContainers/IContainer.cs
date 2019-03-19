using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.BaseContainers
{
	public interface IContainer<TKey,out TValue> : IEnumerable
	{
		int Count { get; }

		TValue GetByKey(TKey key);

		bool ContainsKey(TKey key);
	}
}

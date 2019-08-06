using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SH.Communication
{
	internal class APSSIDs : IAPSSIDs
	{
		private readonly HashSet<string> _ssidsForSearch = new HashSet<string>();

		public IEnumerable<string> List => _ssidsForSearch;

		public bool Contains(string ssid)
		{
			return _ssidsForSearch.Any(s => s == ssid);
		}

		public void Add(string ssid)
		{
			if( !_ssidsForSearch.Add(ssid))
			{
				throw new Exception($"ssid с именем {ssid} уже есть");				
			}
		}

		public void AddRange(IEnumerable<string> ssids)
		{
			foreach(string ssid in ssids)
			{
				Add(ssid);
			}
		}

		public void Remove(string ssid)
		{
			_ssidsForSearch.Remove(ssid);
		}

		public void Clear()
		{
			_ssidsForSearch.Clear();
		}

		public APSSIDs GetCopy()
		{
			APSSIDs copy = new APSSIDs();

			foreach (string ssid in _ssidsForSearch)
			{
				copy.Add(ssid);
			}

			return copy;
		}
	}
}

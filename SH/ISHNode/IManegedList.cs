using SH.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Node
{
	public interface IManegedList<DType> : IEnumerable<DType>
		where DType : IDeviceBase
	{
		int Count { get; }

		int DevicesType { get; }

		IDeviceEditor Editor { get; }

		DType GetByID(int id);

		bool Contains(int id);

		void Add(DType device);

		void AddRange(IEnumerable<DType> devices);
	}
}

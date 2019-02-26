using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.BaseContainers
{
	public interface ICommonContainer
	{
		int Count { get; }

		bool IsPresent<ItemsListType>();

		ItemsListType GetItemsList<ItemsListType>();
	}
}

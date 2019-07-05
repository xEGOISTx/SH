using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SH;
using SHToolKit;
using SHToolKit.DataManagement;
using UWPHelper;
using SHBase;

namespace SHControls.ViewModels
{
	public class NodeViewModel : BaseViewModel
	{
		private readonly INode _node;
		private readonly IDataLoader _loader;
		private readonly ITools _tools;

		public NodeViewModel(INode node, IDataLoader loader, ITools tools)
		{
			_node = node;
			_loader = loader;
			_tools = tools;
		}


		public DevicesViewModel DevicesVM { get; private set; }


		public async Task<IOperationResult> Init()
		{		
			DevicesVM = new DevicesViewModel(_node.DevicesManager, _loader, _tools);
			OnPropertyChanged(nameof(DevicesVM));
			return await DevicesVM.PreInit();
		}
	}
}

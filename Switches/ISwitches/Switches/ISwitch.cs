using SHBase.DevicesBaseComponents;
using System.Threading.Tasks;

namespace Switches
{
	/// <summary>
	/// Обычный переключатель
	/// </summary>
	public interface ISwitch : IBaseSwitch
	{
		/// <summary>
		/// Задачи
		/// </summary>
		ISwitchOutletTaskList Tasks { get; }
	}
}

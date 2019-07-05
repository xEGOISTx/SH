using SHToolKit.DataManagement;
using SHBase;
using SHToolKit;
using SHToolKit.Communication;
using SHToolKit.DevicesManagement;
using System.Threading.Tasks;

namespace SH
{
	public interface IDevicesManager : IDevicesGetter
	{

		/// <summary>
		/// Найти и подключить новые устройства к роутеру
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <returns></returns>
		Task<IFindDevicesOperationResult> FindAndConnectToRouterNewDevicesAsync(IDevicesFinder devicesFinder);

		/// <summary>
		/// Сохраняет устройства в хранилище и распределяет по соответствующим спискам устройств
		/// </summary>
		/// <param name="findDevicesResult">Резултат поиска новых устройств</param>
		/// <param name="loader">Загрузчик. Используется для сохранения устройств в хранилище</param>
		/// <param name="communicator">Средство общения с устройствами</param>
		/// <returns></returns>
		Task<IOperationResult> SaveAndDistributeNewDevices(IFindDevicesOperationResult findDevicesResult, IDevicesLoader loader, ICommunicator communicator);

		/// <summary>
		/// Возвращает неподключенные устройства и информацию о подключении устройств измеривших своё состояние
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <returns></returns>
		Task<IDevicesConnectionInfo> GetDevicesConnectionInfo(IDevicesFinder devicesFinder);

		/// <summary>
		/// Обновить состояния подключения устройств
		/// </summary>
		/// <param name="devsConnInfo">Информация о состоянии подключения устройств</param>
		void RefreshDevicesConnectionState(IDevicesConnectionInfo devsConnInfo);

		/// <summary>
		/// Попытка найти соответствующие устройства на роутере если они подключены
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <param name="devsConnInfo">Информация о состоянии подключения устройств</param>
		/// <returns></returns>
		Task<IFindDevicesOperationResult> FindDevicesAtRouterIfItsConn(IDevicesFinder devicesFinder, IDevicesConnectionInfo devsConnInfo);

		/// <summary>
		/// Синхронизация устройств с найденными устройствами на роутере
		/// </summary>
		/// <param name="devsFromRouterResult"></param>
		/// <returns></returns>
		Task<IOperationResult> DevicesSynchronization(IFindDevicesOperationResult devsFromRouterResult);

		Task<IOperationResult> LoadDataFromRepository(IDataLoader loader);
	}
}

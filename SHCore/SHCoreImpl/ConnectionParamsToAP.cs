namespace SH.Core
{
	/// <summary>
	/// Параметры для подключения к точке доступа
	/// </summary>
	public class ConnectionParamsToAP : IConnectionParamsToAP
	{
		public string SSID { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;
	}
}

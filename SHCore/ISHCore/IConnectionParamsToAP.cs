namespace SH.Core
{
	/// <summary>
	/// Параметры для подкючения к точке доступа
	/// </summary>
	public interface IConnectionParamsToAP
	{
		string SSID { get; }

		string Password { get; }
	}
}

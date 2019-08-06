namespace SH.Communication
{
	/// <summary>
	/// Интерфейс точки доступа
	/// </summary>
	public interface IAP
	{
		string SSID { get; }

		/// <summary>
		/// Установить пароль для подключения к точке доступа
		/// </summary>
		/// <param name="password"></param>
		void SetPasswordToConnected(string password);
	}
}

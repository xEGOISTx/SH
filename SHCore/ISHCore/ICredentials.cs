namespace SH.Core
{
	/// <summary>
	/// Учётные данные
	/// </summary>
	public interface ICredentials
	{
		string Login { get; }

		string Password { get; }
	}
}

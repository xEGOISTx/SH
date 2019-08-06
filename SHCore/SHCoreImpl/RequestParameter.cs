namespace SH.Core
{
	/// <summary>
	/// Параметер для отправки запроса
	/// </summary>
	public class RequestParameter
	{
		public RequestParameter(string name,string value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Название параметра
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Значение параметра
		/// </summary>
		public string Value { get; }
	}
}

using System;
using System.Linq;

namespace SH.Core
{
	/// <summary>
	/// Mac адрес
	/// </summary>
	public class MacAddress
	{
		private readonly string _mac;
		private const int MAC_LENGHT = 17;
		private const int HEX_NUMBERS_COUNT = 6;

		/// <summary>
		/// Инициализация mac адреса
		/// </summary>
		/// <param name="macAddress"></param>
		public MacAddress(string macAddress)
		{
			if (!CheckMac(macAddress,out _mac))
			{
				throw new FormatException();
			}
		}

		/// <summary>
		/// Возвращает mac адрес строкой
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _mac;
		}

		/// <summary>
		/// Возвращает хеш код
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return _mac.GetHashCode();
		}

		public static bool operator ==(MacAddress mac1, MacAddress mac2)
		{
			return mac1.GetHashCode() == mac2.GetHashCode();
		}

		public static bool operator !=(MacAddress mac1, MacAddress mac2)
		{
			return mac1.GetHashCode() != mac2.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is MacAddress mac && this == mac;
		}

		/// <summary>
		/// Проверить mac на корректность
		/// </summary>
		/// <param name="mac"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private bool CheckMac(string mac, out string result)
		{
			result = string.Empty;
			mac.Trim();

			if (mac.Length == MAC_LENGHT)
			{
				string[] hexNumbers = mac.Split(':');

				if (hexNumbers.Count() == HEX_NUMBERS_COUNT)
				{
					try
					{
						foreach(string nexNum in hexNumbers)
						{
							Convert.ToInt32(nexNum, 16);
						}
					}
					catch
					{
						return false;
					}

					result = mac.ToLower();
					return true;
				}
			}
			
			return false;
		}
	}
}

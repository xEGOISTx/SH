using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public class MacAddress
	{
		private readonly string _mac;
		private const int MAC_LENGHT = 17;
		private const int HEX_NUMBERS_COUNT = 6;

		public MacAddress(string macAddress)
		{
			if (!CheckMac(macAddress,out _mac))
			{
				throw new FormatException();
			}
		}

		public override string ToString()
		{
			return _mac;
		}

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

		private bool CheckMac(string mac, out string result)
		{
			result = string.Empty;
			mac.Trim();

			if (mac.Length == MAC_LENGHT)
			{
				string[] hexNumbers = mac.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

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

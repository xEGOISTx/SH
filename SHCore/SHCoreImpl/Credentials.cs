using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Core
{
	/// <summary>
	/// Учётные данные
	/// </summary>
	public class Credentials : ICredentials
	{
		public string Login { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;
	}
}

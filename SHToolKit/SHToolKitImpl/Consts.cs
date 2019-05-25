using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public class Consts
	{
		internal const string ESP = "Test";

		public static readonly IPAddress ZERO_IP = IPAddress.Parse("0.0.0.0");

		internal const string IS_CONNECTED = "Подключено";

		/// <summary>
		/// Путь к корню распознавателя речи
		/// </summary>
		internal const string SR_ROOT_FOLDER = @"SHToolKitImpl\SpeechRecognition\";
	}
}

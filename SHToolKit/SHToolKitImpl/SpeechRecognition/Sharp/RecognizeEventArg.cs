using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.SpeechRecognition
{
	/// <summary>
	/// Обработчик распознавания
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void RecognizeEventHandler(object sender, RecognizeEventArg e);

	/// <summary>
	/// Предоставляет результат распознавания
	/// </summary>
	public class RecognizeEventArg
	{
		internal RecognizeEventArg(string recognizeResult)
		{
			RecognizeResult = recognizeResult;
		}

		/// <summary>
		/// Результат распознавания
		/// </summary>
		public string RecognizeResult { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SHToolKit.SpeechRecognition
{
	public class SpeechRecognizer
	{
		private IntPtr _ps = IntPtr.Zero;
		private IntPtr _ad = IntPtr.Zero;
		private readonly string[] _dllNames = new string[3] { "SR.dll", "sphinxbase.dll", "pocketsphinx.dll" };

		/// <summary>
		/// Признак объект инициализирован
		/// </summary>
		public bool IsInit { get; private set; } = false;

		/// <summary>
		/// Признак распознание речи запущено
		/// </summary>
		public bool IsRunRecognition { get; private set; } = false;

		/// <summary>
		/// Принудительное завершение всех процессов распознавания и очистка неупраляемых объектов
		/// </summary>
		public async void Dispose()
		{
			if (IsRunRecognition)
			{
				await StopRecognizeAsync();
			}

			Free();			
		}

		/// <summary>
		/// Инициализация объектов необходимых для распознавания
		/// </summary>
		/// <param name="argFile"></param>
		/// <returns></returns>
		public async Task<bool> Init(string argFile)
		{
			bool checkResult = await CheckDLLs();

			if (!checkResult)
			{
				await CopyDllsToRootFolder();
			}

			if (!IsInit)
			{
				return await Task.Run(() =>
				{
					_ps = SphinxWrapper.InitSphinx(argFile);
					_ad = SphinxWrapper.OpenRecordingDevice(_ps);

					if (_ps != null && _ps != IntPtr.Zero && _ad != null && _ad != IntPtr.Zero)
					{
						IsInit = true;
					}

					return false;
				});
			}

			return true;
		}

		/// <summary>
		/// Уничтожить неуправляемые внутренние объекты. Не выполнится, если запущено распознавание 
		/// </summary>
		public void Free()
		{
			if (IsInit && !IsRunRecognition)
			{
				SphinxWrapper.FreeSphinx(_ps, _ad);
				_ps = IntPtr.Zero;
				_ad = IntPtr.Zero;

				GC.Collect();
				IsInit = false;
			}
		}

		/// <summary>
		/// Запуск процесса распознавания. Что бы получать результаты распознавания необходимо подписаться на  event Recognized
		/// </summary>
		public async void StartRecognizeAsync()
		{
			if (!IsRunRecognition)
			{
				IsRunRecognition = true;
				CoreDispatcher dispatcher = Window.Current.Dispatcher;

				await Task.Run(async () =>
				{
					while (true)
					{
						if (IsInit)
						{
							IntPtr res = SphinxWrapper.RecognizeFromMic(_ps, _ad);
							bool isRun = SphinxWrapper.RecognizerIsRun();

							if (isRun && res != IntPtr.Zero && res != null)
							{
								string result = GetString(res);

								await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
								{
									OnRecognized(result);
								});
							}
							else if (!isRun)
							{
								IsRunRecognition = false;
								break;
							}
							else if (res == null || res == IntPtr.Zero)
							{
								throw new NullReferenceException("Ошибка распознования");
							}
						}
						else
						{
							throw new NullReferenceException("Объект не инициализирован");
						}
					}
				});
			}
			else
			{
				throw new Exception("Попытка запуска боле одного сеанса распознования");
			}
		}

		/// <summary>
		/// Остановить процесс распознавания
		/// </summary>
		/// <returns></returns>
		public async Task<bool> StopRecognizeAsync()
		{
			return await Task.Run(() =>
			{
				SphinxWrapper.StopRecognize();

				while (IsRunRecognition) { }

				return true;
			});
		}


		private string GetString(IntPtr recognizedResult)
		{
			string resUni = Marshal.PtrToStringUni(recognizedResult);
			byte[] uniBytes = Encoding.Unicode.GetBytes(resUni);

			char[] chs = Encoding.UTF8.GetString(uniBytes).ToCharArray();
			string result = string.Empty;

			foreach (char ch in chs)
			{
				if (ch != '\0')
				{
					result += ch;
				}
				else
				{
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Проверить наличие необходимых dll в корне проэкта
		/// </summary>
		/// <returns></returns>
		private async Task<bool> CheckDLLs()
		{
			foreach(string dllName in _dllNames)
			{
				IStorageItem f = await Windows.ApplicationModel.Package.Current.InstalledLocation.TryGetItemAsync(dllName);
				if(f == null)
				{
					return false;
				}
			}

			return true;
		}

		private async Task<bool> CopyDllsToRootFolder()
		{
			StorageFolder rootFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

			foreach(string dllName in _dllNames)
			{
				string pathToDll = $"{Consts.SR_ROOT_FOLDER}{dllName}";

				StorageFile dll = await Windows.ApplicationModel.Package.Current.InstalledLocation.TryGetItemAsync(pathToDll) as StorageFile;

				await dll.CopyAsync(rootFolder);
			}

			return true;
		}


		private void OnRecognized(string res)
		{
			Recognized?.Invoke(this, new RecognizeEventArg(res));
		}

		/// <summary>
		/// Возникает если речь распознана
		/// </summary>
		public event RecognizeEventHandler Recognized;
	}
}

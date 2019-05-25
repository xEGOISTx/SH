using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.SpeechRecognition
{
	internal class SphinxWrapper
	{
		private const string _lib = @"SR.dll";

		[DllImport(_lib, EntryPoint = "init_sphinx", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr InitSphinx(string configFile);


		[DllImport(_lib, EntryPoint = "open_recording_device")]
		public static extern IntPtr OpenRecordingDevice(IntPtr ps);


		[DllImport(_lib, EntryPoint = "recognize_from_mic", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr RecognizeFromMic(IntPtr decoder, IntPtr audioDevice);


		[DllImport(_lib, EntryPoint = "free_sphinx")]
		public static extern void FreeSphinx(IntPtr ps, IntPtr audioDevice);


		[DllImport(_lib, EntryPoint = "recognizer_is_run", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool RecognizerIsRun();


		[DllImport(_lib, EntryPoint = "stop_recognize", CallingConvention = CallingConvention.Cdecl)]
		public static extern void StopRecognize();
	}
}

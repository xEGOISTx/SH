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
		[DllImport("SR.dll", EntryPoint = "init_sphinx", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr InitSphinx(string configFile);


		[DllImport("SR.dll", EntryPoint = "open_recording_device")]
		public static extern IntPtr OpenRecordingDevice(IntPtr ps);


		[DllImport("SR.dll", EntryPoint = "recognize_from_mic", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr RecognizeFromMic(IntPtr decoder, IntPtr audioDevice);


		[DllImport("SR.dll", EntryPoint = "free_sphinx")]
		public static extern void FreeSphinx(IntPtr ps, IntPtr audioDevice);


		[DllImport("SR.dll", EntryPoint = "recognizer_is_run", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool RecognizerIsRun();


		[DllImport("SR.dll", EntryPoint = "stop_recognize", CallingConvention = CallingConvention.Cdecl)]
		public static extern void StopRecognize();
	}
}

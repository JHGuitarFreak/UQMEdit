using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UQMEdit
{
	partial class Read
	{
		public static FileStream Stream;
		public static Main Window;
		public static byte[] FileBuffer;
		public static byte SaveVersion = 0;
		public static string SaveName;
		public static string TimeDate;

		public static void Open(string FileName, Main window) {
			if (!File.Exists(FileName)) {
				MessageBox.Show("Could not find path: " + FileName);
				return;
			}

			Stream = new FileStream(FileName, FileMode.Open);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = window;

			byte[] HDCheckerArray = new byte[16];
			byte[] CheckerArray = new byte[4];
			byte[] SaveMagicArray = new byte[4];
			byte[] ResFactorArray = new byte[1];

			// Save Checker
			Stream.Seek(Offs.SaveChecker, SeekOrigin.Begin);
			Stream.Read(HDCheckerArray, 0, 15);
			if (Functions.ByteToString(HDCheckerArray, 15) == Vars.SaveCheckerStrHD)
				SaveVersion = 1;
			else {
				Array.Copy(HDCheckerArray, CheckerArray, CheckerArray.Length);
				int LoadChecker = BitConverter.ToInt32(CheckerArray, 0);

				Stream.Seek(Offs.MM.ResFactor, SeekOrigin.Begin);
				Stream.Read(ResFactorArray, 0, 1);

				if (LoadChecker == Vars.SaveFileTag && ResFactorArray[0] > 2)
					SaveVersion = 3;
				else if (LoadChecker == Vars.SaveFileTag)
					SaveVersion = 2;
				else
					SaveVersion = 0;
			}

			Summary();
			Coordinates();

			Stream.Close();
			Stream.Dispose();
		}
	}
}

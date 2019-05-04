using System;
using System.IO;
using System.Text;
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

		public static void Open(string FileName, Main window) {
			if (!File.Exists(FileName)) {
				MessageBox.Show("Could not find path: " + FileName);
				return;
			}

			Stream = new FileStream(FileName, FileMode.Open);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = window;

			byte[] CheckerArray = new byte[4];
			byte[] ResFactorArray = new byte[1];

			// Save Checker			
			int LoadChecker = Functions.ReadOffsetToInt(Offs.SaveChecker, 4);
			if (LoadChecker == Vars.SaveFileTag && Functions.ReadOffset(Offs.MM.ResFactor, 1)[0] > 2) {
				SaveVersion = 3;
			} else if (LoadChecker == Vars.SaveFileTag) {
				SaveVersion = 2;
			} else if (LoadChecker == Vars.SaveTagHD)
				SaveVersion = 1;
			else
				SaveVersion = 0;

			Coordinates();
			Summary();

			Stream.Close();
			Stream.Dispose();
		}
	}
}
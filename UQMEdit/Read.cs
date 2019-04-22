using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UQMEdit
{
	class Read
	{
		public static FileStream Stream;
		public static Main Window;
		public static byte[] FileBuffer;

		public static void Open(string FileName, Main window) {
			if (!File.Exists(FileName)) {
				MessageBox.Show("Could not find path: " + FileName);
				return;
			}

			Stream = new FileStream(FileName, FileMode.Open);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = window;

			//Summary();
			//Coordinates();

			Stream.Close();
			Stream.Dispose();
		}
	}
}

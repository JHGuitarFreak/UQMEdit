using System.IO;
using System.Windows.Forms;

namespace UQMEdit
{
	partial class Write
	{
		public static FileStream Stream;
		public static Main Window;
		public static int Num;
		public static uint uNum;
		public static byte[] FileBuffer;

		public static void Save(string FileName, Main WindowRef) {

			Stream = new FileStream(FileName, FileMode.OpenOrCreate);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = WindowRef;

			Summary();
			Coordinates();

			Stream.Close();
			Stream.Dispose();
		}
	}
}
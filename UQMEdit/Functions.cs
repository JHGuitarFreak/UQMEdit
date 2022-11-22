using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace UQMEdit
{
	class Functions
	{
		// Read
		public static byte[] ReadByteArray(int offset, int array_length)
		{
			Read.Stream.Seek(offset, SeekOrigin.Begin);
			Read.Stream.Read(Read.FileBuffer, 0, array_length);

			Vars.LastOffset += array_length;

			return Read.FileBuffer;
		}

		public static byte ReadByte ()
		{
			return ReadByteArray (Vars.LastOffset, 1)[0];
		}

		public static sbyte ReadSByte ()
		{
			return (sbyte)ReadByteArray (Vars.LastOffset, 1)[0];
		}

		public static ushort ReadUShort ()
		{
			return BitConverter.ToUInt16 (ReadByteArray (Vars.LastOffset, 2), 0);
		}

		public static short ReadShort ()
		{
			return BitConverter.ToInt16 (ReadByteArray (Vars.LastOffset, 2), 0);
		}

		public static uint ReadUInt ()
		{
			return BitConverter.ToUInt32 (ReadByteArray (Vars.LastOffset, 4), 0);
		}

		public static int ReadInt ()
		{
			return BitConverter.ToInt32 (ReadByteArray (Vars.LastOffset, 4), 0);
		}

		public static string ReadStr (int count)
		{
			byte[] temp = ReadByteArray (Vars.LastOffset, count);
			temp[count] = 0; // Properly terminate the string

			return Encoding.Default.GetString (temp);
		}

		// Write

		public static void WriteSByteArray (int offset, int i, int array_length)
		{
			Write.Stream.Seek (offset, SeekOrigin.Begin);
			Write.FileBuffer = BitConverter.GetBytes (i);
			Write.Stream.Write (Write.FileBuffer, 0, array_length);

			Vars.LastOffset += array_length;
		}

		public static void WriteByteArray (int offset, uint i, int array_length)
		{
			Write.Stream.Seek (offset, SeekOrigin.Begin);
			Write.FileBuffer = BitConverter.GetBytes (i);
			Write.Stream.Write (Write.FileBuffer, 0, array_length);

			Vars.LastOffset += array_length;
		}

		public static void WriteByte (byte b)
		{
			WriteByteArray (Vars.LastOffset, b, 1);
		}

		public static void WriteShort (short s)
		{
			WriteSByteArray (Vars.LastOffset, s, 2);
		}

		public static void WriteInt (int i)
		{
			WriteSByteArray (Vars.LastOffset, i, 4);
		}

		public static byte[] StringToByteArray (string String, int MaxLength)
		{
			char[] charArr = String.ToCharArray ();
			byte[] bytes = new byte[MaxLength];
			for (int i = 0; i < MaxLength; i++)
			{
				if (i < charArr.Length)
					bytes[i] = Convert.ToByte (charArr[i]);
				else
					bytes[i] = 0;
			}
			return bytes;
		}

		public static void WriteString (string String, int ByteLength)
		{
			Write.Stream.Seek (Vars.LastOffset, SeekOrigin.Begin);
			Write.FileBuffer = StringToByteArray (String, ByteLength);
			Write.Stream.Write (Write.FileBuffer, 0, ByteLength);

			Vars.LastOffset += ByteLength;
		}

		public static int HSCoordChecker (int Old, int New)
		{
			if (Vars.SaveVersion == 0 || Vars.SaveVersion == 3)
				return Old;
			else
				return New;
		}

		public static int RoundingError (int div)
		{
			return (div >> 1);
		}

		public static int LogXToUniverse (int LogX)
		{
			int UniverseUnits = HSCoordChecker (Vars.UniverseUnitsOld, Vars.UniverseUnits);
			int LogUnits = HSCoordChecker (Vars.LogUnitsXOld, Vars.LogUnits);
			return (LogX * UniverseUnits + RoundingError (LogUnits)) / LogUnits;
		}
		public static int LogYToUniverse (int LogY)
		{
			int LogUnits = HSCoordChecker (Vars.LogUnitsYOld, Vars.LogUnits);
			return Vars.MaxUniverse - ((LogY * Vars.UniverseUnits + RoundingError (LogUnits)) / LogUnits);
		}

		public static int UniverseToLogX (int UniverseX)
		{
			UniverseX -= HSCoordChecker (3, 0);
			int UniverseUnits = HSCoordChecker (Vars.UniverseUnitsOld, Vars.UniverseUnits);
			int LogUnits = HSCoordChecker (Vars.LogUnitsXOld, Vars.LogUnits);
			return (UniverseX * Vars.LogUnits + RoundingError (Vars.UniverseUnits)) / Vars.UniverseUnits;
		}
		public static int UniverseToLogY (int UniverseY)
		{
			int LogUnits = HSCoordChecker (Vars.LogUnitsYOld, Vars.LogUnits);
			return ((Vars.MaxUniverse - UniverseY) * LogUnits + RoundingError (Vars.UniverseUnits)) / Vars.UniverseUnits;
		}
	}

	public class Utilities
	{
		private static void RecursiveResetForm (Control control)
		{
			if (control.HasChildren)
			{
				foreach (Control subControl in control.Controls)
				{
					RecursiveResetForm (subControl);
				}
			}
			switch (control.GetType ().Name)
			{
				case "TextBox":
					TextBox textBox = (TextBox)control;
					textBox.Text = null;
					break;

				case "ComboBox":
					ComboBox comboBox = (ComboBox)control;
					if (comboBox.Items.Count > 0)
						comboBox.SelectedIndex = 0;
					break;

				case "CheckBox":
					CheckBox checkBox = (CheckBox)control;
					checkBox.Checked = false;
					break;

				case "ListBox":
					ListBox listBox = (ListBox)control;
					listBox.ClearSelected ();
					break;

				case "NumericUpDown":
					NumericUpDown numericUpDown = (NumericUpDown)control;
					numericUpDown.Value = 0;
					break;

				case "TrackBar":
					TrackBar trackBar = (TrackBar)control;
					trackBar.Value = 0;
					break;
			}
		}

		public static void ResetAllControls (Control form)
		{
			foreach (Control control in form.Controls)
			{
				RecursiveResetForm (control);
			}
		}
	}
}
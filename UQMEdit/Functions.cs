using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace UQMEdit
{
	class Functions
	{
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

		public static byte[] ReadByteArray(int offset, int array_length)
		{
			Read.Stream.Seek(offset, SeekOrigin.Begin);
			Read.Stream.Read(Read.FileBuffer, 0, array_length);

			Vars.LastOffset += array_length;

			return Read.FileBuffer;
		}

		public static byte[] read_8 (int offset)
		{
			return ReadByteArray (offset, 1);
		}

		public static byte[] read_16 (int offset)
		{
			return ReadByteArray (offset, 2);
		}

		public static byte[] read_32 (int offset)
		{
			return ReadByteArray (offset, 4);
		}

		public static byte ReadByte (int offset)
		{
			return read_8 (offset)[0];
		}

		public static sbyte ReadSByte (int offset)
		{
			return (sbyte)read_8 (offset)[0];
		}

		public static ushort ReadUShort (int offset)
		{
			return BitConverter.ToUInt16 (read_16 (offset), 0);
		}

		public static short ReadShort (int offset)
		{
			return BitConverter.ToInt16 (read_16 (offset), 0);
		}

		public static uint ReadUInt (int offset)
		{
			return BitConverter.ToUInt32 (read_32 (offset), 0);
		}

		public static int ReadInt (int offset)
		{
			return BitConverter.ToInt32 (read_32 (offset), 0);
		}

		public static string ReadStr (int offset, int count)
		{
			return Encoding.Default.GetString (ReadByteArray (offset, count));
		}

		public static byte[] ReadOffset (int Offset, int ByteLength, bool Reverse = false)
		{
			Read.Stream.Seek (Offset, !Reverse ? SeekOrigin.Begin : SeekOrigin.End);
			Read.Stream.Read (Read.FileBuffer, 0, ByteLength);
			return Read.FileBuffer;
		}
		public static int ReadOffsetToInt (int Offset, int ByteLength, int Is16or32 = 32, bool Reverse = false)
		{
			byte[] Buffer = ReadOffset (Offset, ByteLength, Reverse);
			int Query = (Is16or32 == 16) ? BitConverter.ToUInt16 (Buffer, 0) : BitConverter.ToInt32 (Buffer, 0);
			return Query;
		}

		public static void WriteOffset (int Offset, decimal SpinnerValue, int ByteLength, uint Limit, bool IsUINT = false)
		{
			Write.Stream.Seek (Offset, SeekOrigin.Begin);
			if (IsUINT)
			{
				Write.uNum = Decimal.ToUInt32 (SpinnerValue);
				if (Write.uNum >= Limit)
				{
					Write.uNum = Limit;
				}
				Write.FileBuffer = BitConverter.GetBytes (Write.uNum);
			}
			else
			{
				Write.Num = Decimal.ToInt32 (SpinnerValue);
				if (Write.Num >= Limit)
				{
					Write.Num = (int)Limit;
				}
				Write.FileBuffer = BitConverter.GetBytes (Write.Num);
			}
			Write.Stream.Write (Write.FileBuffer, 0, ByteLength);
		}

		public static void WriteOffsetString (int Offset, string String, int ByteLength)
		{
			Write.Stream.Seek (Offset, SeekOrigin.Begin);
			Write.FileBuffer = StringToByteArray (String, ByteLength);
			Write.Stream.Write (Write.FileBuffer, 0, ByteLength);
		}

		public static int OffsPick (int HD, int MegaMod, int Core = 0)
		{
			switch (Read.SaveVersion)
			{
				case 3:
					return (Core > 0 ? Core : MegaMod);
				case 2:
					return MegaMod;
				case 1:
					return HD;
				case 0:
				default:
					return (HD - 48);
			}
		}

		public static int HSCoordChecker (int Old, int New)
		{
			if (Read.SaveVersion == 0 || Read.SaveVersion == 3)
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
}
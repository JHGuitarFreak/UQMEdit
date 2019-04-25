using System;
using System.IO;

namespace UQMEdit
{
	class Functions
	{
		public static byte[] StringToByte(string String, int arrayLength) {
			byte[] array = new byte[Math.Max(arrayLength, String.Length)];
			for (int i = 0; i < arrayLength; i++) {
				array[i] = 0;
			}
			for (int j = 0; j < String.Length; j++) {
				array[j] = (byte)String[j];
			}
			return array;
		}
		
		public static byte[] ReadOffset(int Offset, int ByteLength, bool Reverse = false) {
			Read.Stream.Seek(Offset, !Reverse ? SeekOrigin.Begin : SeekOrigin.End);
			Read.Stream.Read(Read.FileBuffer, 0, ByteLength);
			return Read.FileBuffer;
		}
		public static int ReadOffsetToInt(int Offset, int ByteLength, int Is16or32 = 32, bool Reverse = false) {
			byte[] Buffer = ReadOffset(Offset, ByteLength, Reverse);
			int Query = (Is16or32 == 16) ? BitConverter.ToUInt16(Buffer, 0) : BitConverter.ToInt32(Buffer, 0);
			return Query;
		}
		public static bool ReadGameState(int Offset) {
			bool ReadBool;
			byte[] Buffer = ReadOffset(Offset, 1);
			ReadBool = (Buffer[0] > 0) ? true : false;
			return ReadBool;
		}

		public static void WriteOffset(int Offset, decimal SpinnerValue, int ByteLength, int Limit) {
			Write.Stream.Seek(Offset, SeekOrigin.Begin);
			Write.Num = Decimal.ToInt32(SpinnerValue);
			if (Write.Num >= Limit) {
				Write.Num = Limit;
			}
			Write.FileBuffer = BitConverter.GetBytes(Write.Num);
			Write.Stream.Write(Write.FileBuffer, 0, ByteLength);
		}
		public static void WriteOffsetBool(int Offset, bool Checked, bool IsResearch = false) {
			Write.Stream.Seek(Offset, SeekOrigin.Begin);
			if (IsResearch == true) {
				Write.Num = (Checked == true) ? 36 : 4;
			} else {
				Write.Num = (Checked == true) ? 255 : 254;
			}
			Write.FileBuffer = BitConverter.GetBytes(Write.Num);
			Write.Stream.Write(Write.FileBuffer, 0, 1);
		}

		public static int OffsPick(int HD, int MegaMod) {
			switch (Read.SaveVersion) {
				case 3:
				case 2:
					return MegaMod;
				case 1:
					return HD;
				case 0:
				default:
					return (HD - 48);
			}
		}

		public static int HSCoordChecker(int Old, int New) {
			if (Read.SaveVersion == 0 || Read.SaveVersion == 3)
				return Old;
			else
				return New;
		}

		public static int RoundingError(int div) {
			return (div >> 1);
		}

		public static int LogXToUniverse(int LogX) {
			int UniverseUnits = HSCoordChecker(Vars.UniverseUnitsOld, Vars.UniverseUnits);
			int LogUnits = HSCoordChecker(Vars.LogUnitsXOld, Vars.LogUnits);
			return ((LogX * UniverseUnits + RoundingError(LogUnits)) / LogUnits);
		}
		public static int LogYToUniverse(int LogY) {
			int LogUnits = HSCoordChecker(Vars.LogUnitsYOld, Vars.LogUnits);
			return (Vars.MaxUniverse - ((LogY * Vars.UniverseUnits + RoundingError(LogUnits)) / LogUnits));
		}

		public static int UniverseToLogX(int UniverseX) {
			int UniverseUnits = HSCoordChecker(Vars.UniverseUnitsOld, Vars.UniverseUnits);
			int LogUnits = HSCoordChecker(Vars.LogUnitsXOld, Vars.LogUnits);
			return ((UniverseX * Vars.LogUnits + RoundingError(Vars.UniverseUnits)) / Vars.UniverseUnits);
		}
		public static int UniverseToLogY(int UniverseY) {
			int LogUnits = HSCoordChecker(Vars.LogUnitsYOld, Vars.LogUnits);
			return (((Vars.MaxUniverse - UniverseY) * LogUnits + RoundingError(Vars.UniverseUnits)) / Vars.UniverseUnits);
		}
	}
}

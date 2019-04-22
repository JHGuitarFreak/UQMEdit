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
		public static string ByteToString(byte[] byteArray, int length) {
			char[] array = new char[byteArray.Length + 1];
			for (int i = 0; i < byteArray.Length; i++) {
				array[i] = (char)byteArray[i];
			}
			array[array.Length - 1] = '\0';
			return new string(array).Substring(0, length);
		}

		public static int ReadOffset(int Offset, int ByteLength, int Is16or32 = 32) {
			Read.Stream.Seek(Offset, SeekOrigin.Begin);
			Read.Stream.Read(Read.FileBuffer, 0, ByteLength);
			int Query = (Is16or32 == 16) ? BitConverter.ToUInt16(Read.FileBuffer, 0) : BitConverter.ToInt32(Read.FileBuffer, 0);
			return Query;
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

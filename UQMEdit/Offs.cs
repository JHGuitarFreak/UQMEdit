namespace UQMEdit
{
	class Offs
	{
		public const byte SaveChecker = 0;
		public const byte SaveNameMagic = 8;

		public class HD
		{
			public const byte SaveName = 16;

			public const byte LogX = 48;
			public const byte LogY = 52;

			public const byte ResUnits = 56;
			public const byte Fuel = 60;
			public const byte SiSCrew = 64;

			public const byte TotalMinerals = 66;

			public const byte BioData = 68;

			public const byte ModuleSlots = 70;
			public static byte[] DriveSlots = { 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96 };
			public static byte[] JetSlots = { 97, 98, 99, 100, 101, 102, 103, 104 };

			public const byte Landers = 105;

			public const byte Common = 106;
			public const byte Corrosive = 108;
			public const byte BaseMetal = 110;
			public const byte NobleGas = 112;
			public const byte RareEarth = 114;
			public const byte Precious = 116;
			public const byte Radioactive = 118;
			public const byte Exotic = 120;
			public static byte[] Minerals = { 106, 108, 110, 112, 114, 116, 118, 120 };

			public const byte ShipName = 122;
			public const byte CaptainName = 138;

			public const byte NearestPlanet = 154;
			public const byte Status = 172;

			public const byte LanderMods = 173;

			public static byte[] Date = { 174, 175, 176 };

			public const byte Credits = 178;

			public static byte[] Escorts = { 180, 182 };

			public static byte[] Devices = { 181, 194 };

			public const byte ResFactor = 210;
		}

		public class MM
		{
			public const byte LogX = 12;
			public const byte LogY = 16;

			public const byte ResUnits = 20;
			public const byte Fuel = 24;
			public const byte SiSCrew = 28;

			public const byte TotalMinerals = 30;
			public const byte BioData = 32;

			public const byte ModuleSlots = 34;
			public static byte[] DriveSlots = { 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };
			public static byte[] JetSlots = { 61, 62, 63, 64, 65, 66, 67, 68 };

			public const byte Landers = 69;

			public const byte Common = 70;
			public const byte Corrosive = 72;
			public const byte BaseMetal = 74;
			public const byte NobleGas = 76;
			public const byte RareEarth = 78;
			public const byte Precious = 80;
			public const byte Radioactive = 82;
			public const byte Exotic = 84;
			public static byte[] Minerals = { 70, 72, 74, 76, 78, 80, 82, 84 };

			public const byte ShipName = 86;
			public const byte CaptainName = 102;
			public const byte NearestPlanet = 118;

			public const byte Difficulty = 134;
			public const byte Extended = 135;
			public const byte Nomad = 136;
			public const byte CustomSeed = 137;

			public const byte Status = 141;

			public const byte LanderMods = 142;

			public static byte[] Date = { 143, 144, 145 };

			public const byte Credits = 147;

			public static byte[] Escorts = { 149, 151 };

			public static byte[] Devices = { 150, 163 };

			public const byte ResFactor = 179;
			public static byte SaveName = 180;
		}

		public class Core
		{
			public const byte Status = 134;

			public const byte LanderMods = 135;

			public static byte[] Date = { 136, 137, 138 };

			public const byte Credits = 140;

			public static byte[] Escorts = { 142, 144 };

			public static byte[] Devices = { 143, 156 };

			public static byte SaveName = 172;
		}
	}
}
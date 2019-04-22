using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace UQMEdit
{
	class Offs
	{
		public const byte SaveChecker = 0;
		public const byte SaveNameMagic = 8;

		public class HD
		{
			public const byte SaveName = 16;

			public const byte StarCoordX = 48;
			public const byte StarCoordY = 52;

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

			public const byte ShipName = 122;
			public const byte CommanderName = 138;

			public const byte NearestPlanet = 154;
			public const byte Status = 172;

			public const byte LanderMods = 173;

			public const byte StarDate = 174;

			public const byte Credits = 178;

			public static byte[] Escorts = { 180, 182 };

			public static byte[] Devices = { 181, 194 };

			public const byte ResFactor = 210;
		}

		public class MM
		{

			public const byte StarCoordX = 0x0C;
			public const byte StarCoordY = 0x10;

			public const byte ResUnits = 0x14;
			public const byte Fuel = 0x18;
			public const byte SiSCrew = 0x1C;

			public const byte TotalMinerals = 0x1E;
			public const byte BioData = 0x20;

			public const byte ModuleSlots = 0x22;
			public const byte DriveSlots = 0x32;
			public const byte JetSlots = 0x3D;

			public const byte Landers = 0x45;

			public const byte Common = 0x46;
			public const byte Corrosive = 0x48;
			public const byte BaseMetal = 0x4A;
			public const byte NobleGas = 0x4C;
			public const byte RareEarth = 0x4E;
			public const byte Precious = 0x50;
			public const byte Radioactive = 0x52;
			public const byte Exotic = 0x54;

			public const byte ShipName = 0x56;
			public const byte CommanderName = 0x66;
			public const byte NearestPlanet = 0x76;
			public const byte Status = 0x86;

			public const byte LanderMods = 0x87;

			public const byte StarDate = 0x88;

			public const byte Credits = 0x8C;

			public static byte[] Escorts = { 0x8E, 0x90 };

			public static byte[] Devices = { 0x8F, 0x9C };

			public const byte ResFactor = 0xAC;
			public const byte SaveName = 0xAD;

			public const sbyte CustomSeed = -4;
		}
	}
}

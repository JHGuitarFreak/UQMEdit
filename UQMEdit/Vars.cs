using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UQMEdit
{
	class Vars
	{
		public const string SaveCheckerStrHD = "superbutcherX !";
		public const int SaveFileTag = 0x01534d55;     // "UMS\x01": UQM Save version 1
		public const int MegaModTag = 0x4147454D;       // "MEGA"

		// HS Coord Vars
		public const int MaxUniverse = 9999;

		public const int UniverseUnitsOld = 6250;
		public const int UniverseUnits = 625;

		public const int LogUnits = 10000;
		public const int LogUnitsXOld = 99840;
		public const int LogUnitsYOld = 12000;

		public const byte SaveNameSize = 64;

		public static string[] deviceName = new string[]
		{
			"Portal Spawner #1",
			"Talking Pet",
			"Utwig Bomb",
			"Sun Device",
			"Rosy Sphere",
			"Aqua Helix",
			"Clear Spindle",
			"Ultron (Broken)",
			"Ultron (Semi-repaired)",
			"Ultron (Mostly repaired)",
			"Ultron (Complete)",
			"Shofixti Maidens",
			"Umgah HyperCaster",
			"Burvixese HyperCaster",
			"Data Plate #1",
			"Data Plate #2",
			"Data Plate #3",
			"Taalo Shield",
			"Egg Case Fragment #1",
			"Egg Case Fragment #2",
			"Egg Case Fragment #3",
			"Syreen Shuttle",
			"VUX Beast",
			"Destruction Code",
			"Ur-Quan Warp Pod",
			"Artifact #2",
			"Artifact #3",
			"Moon Base",
			"Talking Pet (Marked)",
			"Sun Device (Marked)",
			"Burvixese HyperCaster (Marked)",
			"Taalo Shield (Marked)",
			"Egg Case Fragment (Marked)",
			"Empty (Marked)"
		};
		public static string[] escortNames = new string[]
		{
			"Arilou Skiff",
			"Chmmr Avatar",
			"Earthling Cruiser",
			"Orz Nemesis",
			"Pkunk Fury",
			"Shofixti Scout",
			"Spathi Eluder",
			"Supox Blade",
			"Thraddash Torch",
			"Utwig Jugger",
			"VUX Intruder",
			"Yehat Terminator Ro",
			"Melnorme Trader",
			"Druuge Mauler",
			"Ilwrath Avenger",
			"Mycon Podship",
			"Slylandro Probe",
			"Umgah Drone",
			"Kzer-Za Dreadnought",
			"ZFP Stinger",
			"Syreen Penetrator",
			"Kohr-Ah Marauder",
			"Yehat Terminator Re",
			"Probe/Samatra",
			"Empty"
		};

		public static string[] statusName = new string[]
		{
			"Super Melee",
			"Last Battle",
			"Encounter",
			"Hyperspace",
			"Interplanetary",
			"Last Battle Won",
			"Quasispace",
			"Planet Orbit",
			"Starbase",
			"Unknown"
		};
	}

	internal class Modules
	{
		public byte HexCode {
			get;
			private set;
		}
		public string Text {
			get;
			private set;
		}
		public Modules(byte hexcode, string text) {
			this.HexCode = hexcode;
			this.Text = text;
		}
		public static object[] CreateModules() {
			return new object[]
			{
				new Modules(22, "Empty Module"),
				//new Modules(0, "Lander"),
				//new Modules(1, "Antimat Thruster"),
				//new Modules(2, "Turning Jets"),
				new Modules(3, "Crew Pod"),
				new Modules(4, "Storage Bay"),
				new Modules(5, "Fuel Tank"),
				new Modules(6, "High-Ef Fuelsys"),
				new Modules(7, "Dynamo Unit"),
				new Modules(8, "Shiva Furnace"),
				new Modules(9, "Ion-Bolt Gun"),
				new Modules(10, "Fusion Blaster"),
				new Modules(11, "Hellbore Cannon"),
				new Modules(12, "Tracking System"),
				new Modules(13, "Point Defense"),
				new Modules(14, "Bomb Part 1"),
				new Modules(15, "Bomb Part 2"),
				new Modules(16, "Crystal Part 1"),
				new Modules(17, "Crystal Part 2"),
				new Modules(18, "Crystal Part 3"),
				new Modules(19, "Crystal Part 4")
			};
		}
	}
}
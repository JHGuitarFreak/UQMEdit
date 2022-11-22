using System;

namespace UQMEdit
{
	class Vars
	{
		public static int LastOffset = 0;
		public const uint SAVEFILE_TAG_HD =  0x65707573; // "supe" : HD-mod Beta Save
		public const uint SAVEFILE_TAG =     0x01534d55; // "UMS\x01": UQM Save version 1
		public const uint MEGA_TAG =         0x4147454D; // "MEGA": MegaMod Save version 2
		public const uint MMV3_TAG =         0x33564D4D; // "MMV3": MegaMod Save version 3
		public const uint SUMMARY_TAG =      0x6d6d7553; // "Summ": Summary. Must be first!
		public const uint GLOBAL_STATE_TAG = 0x74536c47; // "GlSt": Global State. Must be 2nd!
		public const uint GAME_STATE_TAG =   0x74536d47; // "GmSt": Game State Bits. Must be 3rd!

		// HS Coord Vars
		public const int MaxUniverse = 9999;

		public const int UniverseUnitsOld = 6250;
		public const int UniverseUnits = 625;

		public const int LogUnits = 10000;
		public const int LogUnitsXOld = 99840;
		public const int LogUnitsYOld = 12000;

		public const byte READ_SPEED_MASK = ((1 << 3) - 1);
		public const byte NUM_READ_SPEEDS = 5;
		public const byte COMBAT_SPEED_SHIFT = 6;
		public const byte COMBAT_SPEED_MASK = (((1 << 2) - 1) << COMBAT_SPEED_SHIFT);
		public const byte NUM_COMBAT_SPEEDS = 4;
		public const byte MUSIC_DISABLED = (1 << 3);
		public const byte SOUND_DISABLED = (1 << 4);
		public const byte CYBORG_ENABLED = (1 << 5);

		public static byte SaveVersion = 0;
		public static uint SaveNameLength = 0;
		public static int  SisNameSize = 0;

		public static void clearVars ()
		{
			LastOffset     = 0;
			SaveVersion    = 0;
			SaveNameLength = 0;
			SisNameSize    = 0;

			SummPtr.clearSummary ();
			SSPtr.clearSisState ();
			GSPtr.clearGameState ();
			ClockPtr.clearClockState ();
		}

		public static string[] DeviceName = new string[]
		{
			"Quasi Portal",
			"Talking Pet",
			"Utwig Bomb",
			"Sun Device",
			"Rosy Sphere",
			"Aqua Helix",
			"Clear Spindle",
			"Broken Ultron",
			"Broken Ultron (Semi-repaired)",
			"Broken Ultron (Mostly repaired)",
			"Perfect Ultron",
			"Shofixti Maidens",
			"Umgah Caster",
			"Burvixese Caster",
			"1 Data Plate",
			"2 Data Plate",
			"3 Data Plate",
			"Taalo Shield",
			"Egg Case",
			"Egg Case",
			"Egg Case",
			"Syreen Shuttle",
			"VUX Beast",
			"Destruct Code",
			"Warp Pod",
			"Wimbli's Trident",
			"Glowing Rod",
			"Moon Base"
		};
		public static string[] ShipNames = new string[]
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
			"Yehat Terminator",
			"Melnorme Trader",
			"Druuge Mauler",
			"Ilwrath Avenger",
			"Mycon Podship",
			"Slylandro Probe",
			"Umgah Drone",
			"Ur-Quan Dreadnought",
			"Zoq-Fot-Pik Stinger",
			"Syreen Penetrator",
			"Kohr-Ah Marauder",
			"Yehat Terminator",
			"Probe/Samatra",
			"Empty"
		};

		public static string[] StatusName = new string[]
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

		public static string[] Difficulties = new string[]
		{
			"Normal",
			"Easy",
			"Hard"
		};
	}

	public class Modules
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
			HexCode = hexcode;
			Text = text;
		}
		public static object[] CreateModules() {
			return new object[]
			{
				new Modules(22, "Empty"),
				new Modules(3, "Crew Pod"),
				new Modules(4, "Storage Bay"),
				new Modules(5, "Fuel Tank"),
				new Modules(6, "High-Eff FuelSys"),
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

	class ClockPtr
	{
		// Clock State
		public static byte   day_index;
		public static byte   month_index;
		public static ushort year_index;
		public static short  tick_count;
		public static short  day_in_ticks;

		// v0.7.0 and HD-mod only
		// cread_ptr (= cread_32)
		// cread_ptr
		// cread_32 NULL
		// DummyLoadQueue ->
		// cread_ptr
		// cread_ptr
		// cread_ptr
		// cread_ptr
		// cread_16 NULL
		// cread_8 NULL
		// cread_8 NULL
		// cread_16 NULL
		// = +34 bytes of padding

		public static void clearClockState ()
		{
			day_index    = 0;
			month_index  = 0;
			year_index   = 0;
			tick_count   = 0;
			day_in_ticks = 0;
		}
	}

	class GSPtr
	{
		// if legacy saves
		// cread_8 dummy8
		// else modern saves
		// read_32 magic GLOBAL_STATE_TAG
		// read_32 magic "75"
		public static byte     glob_flags;
		public static byte     CrewCost;
		public static byte     FuelCost;
		// if not FUEL_COST_RU return false (MegaMod and HD only)

		public static byte[]   ModuleCost   = new byte[20];
		public static byte[]   ElementWorth = new byte[8];
		// if legacy saves cread_ptr
		public static ushort   CurrentActivity;

		// if legacy saves cread_16 NULL
		// Load Clock State

		public static short  autopilot_x;
		public static short  autopilot_y;
		public static short  ip_location_x;
		public static short  ip_location_y;
		public static short  ShipStamp_x;
		public static short  ShipStamp_y;
		public static ushort ShipFacing;
		public static byte   ip_planet;
		public static byte   in_orbit;

		// VELOCITY_DESC velocity
		public static ushort TravelAngle;
		public static short  vector_width;
		public static short  vector_height;
		public static short  fract_width;
		public static short  fract_height;
		public static short  error_width;
		public static short  error_height;
		public static short  incr_width;
		public static short  incr_height;

		// if legacy saves cread_16 NULL
		// else read_32 magic GAME_STATE_TAG

		public static void clearGameState ()
		{
			glob_flags      = 0;
			CrewCost        = 0;
			FuelCost        = 0;
			Array.Clear (ModuleCost, 0, ModuleCost.Length);
			Array.Clear (ElementWorth, 0, ElementWorth.Length);
			CurrentActivity = 0;
			autopilot_x     = 0;
			autopilot_y     = 0;
			ip_location_x   = 0;
			ip_location_y   = 0;
			ip_planet       = 0;
			in_orbit        = 0;
			TravelAngle     = 0;
			vector_width    = 0;
			vector_height   = 0;
			fract_width     = 0;
			fract_height    = 0;
			error_width     = 0;
			error_height    = 0;
			incr_width      = 0;
			incr_height     = 0;
		}
	}

	class SSPtr
	{
		// SisState
		public static int      log_x;
		public static int      log_y;
		public static uint     ResUnits;
		public static uint     FuelOnBoard;
		public static ushort   CrewEnlisted;
		public static ushort   TotalElementMass;
		public static ushort   TotalBioMass;
		public static byte[]   ModuleSlots    = new byte[16];
		public static byte[]   DriveSlots     = new byte[11];
		public static byte[]   JetSlots       = new byte[8];
		public static byte     NumLanders;
		public static ushort[] ElementAmounts = new ushort[8];
		public static string   ShipName;
		public static string   CommanderName;
		public static string   PlanetName;

		// if legacy saves cread_16 NULL
		// else MegaMod Stuff
		public static byte Difficulty;
		public static byte Extended;
		public static byte Nomad;
		public static int  Seed;

		public static void clearSisState ()
		{
			log_x            = 0;
			log_y            = 0;
			ResUnits         = 0;
			FuelOnBoard      = 0;
			CrewEnlisted     = 0;
			TotalElementMass = 0;
			TotalBioMass     = 0;
			Array.Clear (ModuleSlots, 0, ModuleSlots.Length);
			Array.Clear (DriveSlots, 0, DriveSlots.Length);
			Array.Clear (JetSlots, 0, JetSlots.Length);
			NumLanders       = 0;
			Array.Clear (ElementAmounts, 0, ElementAmounts.Length);
			ShipName      = string.Empty;
			CommanderName = string.Empty;
			PlanetName    = string.Empty;
			Difficulty = 0;
			Extended   = 0;
			Nomad      = 0;
			Seed       = 0;
		}
	}

	class SummPtr
	{
		// if legacy HD-mod save read 2 byte identifier then read 4 byte save name
		// else
		// read_32 magic "save file type" tag
		// read_32 magic SUMMARY_TAG
		// read_32 magic (- 160)

		// Load SisState
		public static byte   Activity;
		public static byte   Flags;
		public static byte   day_index;
		public static byte   month_index;
		public static ushort year_index;
		//public static byte   MCreditLo;
		//public static byte   MCreditHi;
		public static ushort MCredit; // For simplicity sake, combine the two previous into one variable
		public static byte   NumShips;
		public static byte   NumDevices;
		public static byte[] ShipList   = new byte[12];
		public static byte[] DeviceList = new byte[16];

		public static byte   res_factor; // HD-only
		// if legacy saves read_8 NULL
		public static string SaveName;   // v0.8.0, HD, and MegaMod

		public static void clearSummary ()
		{
			Activity    = 0;
			Flags       = 0;
			day_index   = 0;
			month_index = 0;
			year_index  = 0;
			MCredit     = 0;
			NumShips    = 0;
			NumDevices  = 0;
			Array.Clear (ShipList, 0, ShipList.Length);
			Array.Clear (DeviceList, 0, DeviceList.Length);
			res_factor  = 0;
			SaveName = string.Empty;
		}
	}
}
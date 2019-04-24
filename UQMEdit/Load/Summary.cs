using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
namespace UQMEdit
{
	partial class Read
	{
		public static string SaveName;
		public static string Date = "";

		public static void Summary() {
			byte[] NameArray = new byte[64];
			byte[] SaveMagicArray = new byte[4];
			byte[] LanderModsArray = new byte[1];
			byte[] NamesArray = new byte[16];
			byte[] array = new byte[256];

			if (SaveVersion >= 2) {
				int Magic;
				int NameSize = 0;
				Magic = Functions.ReadOffsetToInt(Offs.SaveNameMagic, 4);
				NameSize = (Magic - 160);
				SaveName = Encoding.Default.GetString(Functions.ReadOffset(SaveVersion == 3 ? (Offs.MM.SaveName - 1) : Offs.MM.SaveName, NameSize));
			} else if (SaveVersion == 1) {
				Read.SaveName = Encoding.Default.GetString(Functions.ReadOffset(Offs.HD.SaveName, 31));
			} else {
				SaveName = "";
			}

			// Time & Date
			int Month, Year, Day = 0;
			Day = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Date[0], Offs.MM.Date[0]), 1)[0];
			Month = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Date[1], Offs.MM.Date[1]), 1)[0];
			Year = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.Date[2], Offs.MM.Date[2]), 2, 16);
			Date = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month).ToUpper() + " " + Day + "·" + Year;

			// Ship Name
			Window.ShipName.Text = Encoding.Default.GetString(Functions.ReadOffset(Functions.OffsPick(Offs.HD.ShipName, Offs.MM.ShipName), 16));
			// Captain Name
			Window.CommanderName.Text = Encoding.Default.GetString(Functions.ReadOffset(Functions.OffsPick(Offs.HD.CaptainName, Offs.MM.CaptainName), 16));

			// SiS Crew
			Window.ShipCrew.Value = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.SiSCrew, Offs.MM.SiSCrew), 4, 16);
			// Fuel
			decimal fuel = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.Fuel, Offs.MM.Fuel), 4);
			Window.ShipFuel.Value = fuel / 100;
			// Resource Units
			Window.ResUnits.Value = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.ResUnits, Offs.MM.ResUnits), 4);
			Window.Credits.Text = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.Credits, Offs.MM.Credits), 2, 16).ToString();
			// Life Forms
			Window.BioData.Value = Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.BioData, Offs.MM.BioData), 2, 16);
			// Landers
			Window.Landers.Value = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Landers, Offs.MM.Landers), 1)[0];

			//  Lander Mods
			int LanderMods = Functions.ReadOffset(Functions.OffsPick(Offs.HD.LanderMods, Offs.MM.LanderMods), 1)[0];
			bool LanderModsBool(int OtherValue, bool Bomb = false) {
				if (!Bomb)
					return ((LanderMods | 128) & OtherValue) != 0 ? true : false;
				return (LanderMods & OtherValue) != 0 ? true : false;
			}
			Window.IsBomb.Checked = LanderModsBool(128, true);
			Window.Resistant.Checked = LanderModsBool(1);
			Window.Quake.Checked = LanderModsBool(2);
			Window.Lightning.Checked = LanderModsBool(4);
			Window.HotSpot.Checked = LanderModsBool(8);
			Window.Speed.Checked = LanderModsBool(16);
			Window.Capacity.Checked = LanderModsBool(32);
			Window.Weapons.Checked = LanderModsBool(64);

			//  Devices
			int NumberOfDevices = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Devices[0], Offs.MM.Devices[0]), 1)[0];
			object[] DevicesIntArray = new object[NumberOfDevices];
			byte[] DevicesArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Devices[1], Offs.MM.Devices[1]), NumberOfDevices);
			for (int i = 0; i < NumberOfDevices; i++) {
				if (DevicesArray[i] < 0 || DevicesArray[i] >= Vars.DeviceName.Length)
					DevicesIntArray[i] = "Please report this [0x" + DevicesArray[i].ToString("X2") + "]";
				else
					DevicesIntArray[i] = Vars.DeviceName[DevicesArray[i]];
			}
			Window.Devices.Items.Clear();
			Window.Devices.Items.AddRange(DevicesIntArray);

			//  Custom Seed
			if (SaveVersion == 2) {
				Window.CustomSeed.Text = Functions.ReadOffsetToInt(Offs.MM.CustomSeed, 4, 32, true).ToString();
			}
		}
	}
}

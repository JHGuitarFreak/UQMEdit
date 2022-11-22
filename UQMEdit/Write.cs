using System.IO;
using System.Windows.Forms;
using System;

namespace UQMEdit
{
	public class Write
	{
		public static FileStream Stream;
		public static Main Window;
		public static int Num;
		public static uint uNum;
		public static byte[] FileBuffer;

		public static void SaveClockState ()
		{
		}

		public static void SaveGameState ()
		{

		}

		public static void SaveSisState ()
		{
			{
				int snum;

				// UniverseX
				decimal UniverseX = Window.UniverseX.Value * 10;
				snum = Functions.UniverseToLogX (decimal.ToInt32 (UniverseX));
				snum = snum > 159735 ? 159735 : snum;
				Functions.WriteInt (snum);

				// UniverseY
				decimal UniverseY = Window.UniverseY.Value * 10;
				snum = Functions.UniverseToLogY (decimal.ToInt32 (UniverseY));
				snum = snum > 191990 ? 191990 : snum;
				Functions.WriteInt (snum);
			}

			Functions.WriteInt ((int)Window.ResUnits.Value);
			Functions.WriteInt ((int)(Window.ShipFuel.Value * 100));
			Functions.WriteShort ((short)Window.ShipCrew.Value);
			Functions.WriteShort ((short)Window.TotalMinerals.Value);
			Functions.WriteShort ((short)Window.BioData.Value);

			{
				// Flagship Modules
				byte Mods;
				foreach (object Mod in Window.ModulesBox.Controls)
				{
					if (Mod is ComboBox)
					{
						int Index = (Mod as ComboBox).SelectedIndex;

						if (Index > 0)
							Mods = (byte)(Index + 2);
						else
							Mods = 22;

						Functions.WriteByte (Mods);
					}
				}

				// Thrusters
				byte Thrusters;
				foreach (object Thruster in Window.ThrusterBox.Controls)
				{
					if (Thruster is CheckBox)
					{
						bool Index = (Thruster as CheckBox).Checked;
						Thrusters = (byte)(Index ? 1 : 20);
						Functions.WriteByte (Thrusters);
					}
				}

				// Jets
				byte Jets;
				foreach (object Jet in Window.JetsBox.Controls)
				{
					if (Jet is CheckBox)
					{
						bool Index = (Jet as CheckBox).Checked;
						Jets = (byte)(Index ? 2 : 21);
						Functions.WriteByte (Jets);
					}
				}
			}

			Functions.WriteByte ((byte)Window.Landers.Value);

			Functions.WriteShort ((short)Window.Common.Value);
			Functions.WriteShort ((short)Window.Corrosive.Value);
			Functions.WriteShort ((short)Window.BaseMetal.Value);
			Functions.WriteShort ((short)Window.NobleGas.Value);
			Functions.WriteShort ((short)Window.RareEarth.Value);
			Functions.WriteShort ((short)Window.Precious.Value);
			Functions.WriteShort ((short)Window.Radioactive.Value);
			Functions.WriteShort ((short)Window.Exotic.Value);

			Functions.WriteString (Window.ShipName.Text, Vars.SisNameSize);
			Functions.WriteString (Window.CommanderName.Text, Vars.SisNameSize);

			Vars.LastOffset += (Vars.SisNameSize / 8); // Planet Name

			if (Vars.SaveVersion == 2 || Vars.SaveVersion == 4)
			{
				Vars.LastOffset += 1; // Difficulty
				Vars.LastOffset += 1; // Extended
				Vars.LastOffset += 1; // Nomad
				Vars.LastOffset += 4; // Seed
			}
		}

		public static void SaveSummary ()
		{
			if (Vars.SaveVersion > 1)
			{
				Vars.LastOffset += 4; // SUMMARY_TAG
				Vars.LastOffset += 4; // 160 + length of SaveName
			}

			SaveSisState ();

			Vars.LastOffset += 1; // Activity
			Vars.LastOffset += 1; // Flags
			Vars.LastOffset += 1; // day_index
			Vars.LastOffset += 1; // month_index
			Vars.LastOffset += 2; // year_index
			Vars.LastOffset += 2; // MCredit
			Vars.LastOffset += 1; // NumShips
			Vars.LastOffset += 1; // NumDevices
			Vars.LastOffset += 12; // ShipList
			Vars.LastOffset += 16; // DeviceList

			if (Vars.SaveVersion <= 1)
				Vars.LastOffset += 2; // Padding


		}

		public static void SaveGame ()
		{
			if (Vars.SaveVersion > 1)
				Vars.LastOffset += 4;
			else if (Vars.SaveVersion == 1)
				Vars.LastOffset += 48;

			SaveSummary ();

			if (Vars.SaveVersion > 1)
				SaveGameState ();
		}

		public static void Save (string FileName, Main WindowRef)
		{
			Stream = new FileStream (FileName, FileMode.OpenOrCreate);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = WindowRef;

			Vars.LastOffset = 0;

			SaveGame ();

			Stream.Close();
			Stream.Dispose();
		}
	}
}
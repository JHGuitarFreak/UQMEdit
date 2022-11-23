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
			Functions.WriteByte ((byte)Window.DayIndex.Value);
			Functions.WriteByte ((byte)Window.MonthIndex.Value);
			Functions.WriteShort ((short)Window.YearIndex.Value);
			Functions.WriteShort ((short)Window.TickCount.Value);
			Functions.WriteShort ((short)Window.DayInTicks.Value);
		}

		public static void SaveGameState ()
		{
			Vars.LastOffset += 4; // GLOBAL_STATE_FLAG
			Vars.LastOffset += 4; // 75

			{
				// glob_flags

				GSPtr.glob_flags = (byte)((GSPtr.glob_flags & ~Vars.READ_SPEED_MASK) | Window.ReadSpeed.Value);
				if (Window.CombatSpeed.Value > 0)
				{
					GSPtr.glob_flags |= Vars.CYBORG_ENABLED;
					GSPtr.glob_flags = (byte)((GSPtr.glob_flags & ~Vars.COMBAT_SPEED_MASK) | ((Window.CombatSpeed.Value) << Vars.COMBAT_SPEED_SHIFT));
				}
				else
				{
					GSPtr.glob_flags &= (byte)~Vars.CYBORG_ENABLED;
					GSPtr.glob_flags = (byte)((GSPtr.glob_flags & ~Vars.COMBAT_SPEED_MASK) | (0 << Vars.COMBAT_SPEED_SHIFT));
				}

				if (Window.MusicCheckBox.Checked)
					GSPtr.glob_flags |= Vars.MUSIC_DISABLED;
				else
					GSPtr.glob_flags &= (byte)~Vars.MUSIC_DISABLED;

				if (Window.SoundCheckBox.Checked)
					GSPtr.glob_flags |= Vars.SOUND_DISABLED;
				else
					GSPtr.glob_flags &= (byte)~Vars.SOUND_DISABLED;

				Functions.WriteByte (GSPtr.glob_flags);
			}

			Functions.WriteByte ((byte)Window.CrewCost.Value);

			if (Vars.SaveVersion == 3)
				Functions.WriteByte ((byte)Window.FuelCost.Value);
			else
				Vars.LastOffset++;

			Functions.WriteByte ((byte)((int)Window.LanderCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.ThrusterCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.JetCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.CrewPodCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.StorageBayCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.FuelTankCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.HiEffFuelSysCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.DynamoUnitCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.ShivaFurnaceCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.IonBoltGunCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.FusionBlasterCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.HellboreCannonCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.TrackingSystemCost.Value / 50));
			Functions.WriteByte ((byte)((int)Window.PointDefenseCost.Value / 50));

			Vars.LastOffset += 6; // Bomb Parts

			Functions.WriteByte ((byte)Window.CommonWorth.Value);
			Functions.WriteByte ((byte)Window.CorrosiveWorth.Value);
			Functions.WriteByte ((byte)Window.BaseMetalWorth.Value);
			Functions.WriteByte ((byte)Window.NobleGasWorth.Value);
			Functions.WriteByte ((byte)Window.RareEarthWorth.Value);
			Functions.WriteByte ((byte)Window.PreciousMetalWorth.Value);
			Functions.WriteByte ((byte)Window.RadioactiveWorth.Value);
			Functions.WriteByte ((byte)Window.ExoticWorth.Value);

			Vars.LastOffset += 2; // Current Activity

			SaveClockState ();

			Functions.WriteShort ((short)(Window.AutoPilotX.Value == -1 ? -1 : (Window.AutoPilotX.Value * 10)));
			Functions.WriteShort ((short)(Window.AutoPilotY.Value == -1 ? -1 : (Window.AutoPilotY.Value * 10)));
			Functions.WriteShort ((short)Window.IPLocationX.Value);
			Functions.WriteShort ((short)Window.IPLocationY.Value);
			Functions.WriteShort ((short)Window.ShipOriginX.Value);
			Functions.WriteShort ((short)Window.ShipOriginY.Value);
			Functions.WriteShort ((short)Window.ShipFacing.Value);
			Vars.LastOffset++; // ip_planet
			Vars.LastOffset++; // in_orbit

			Functions.WriteShort ((short)Window.TravelAngle.Value);
			Functions.WriteShort ((short)Window.VectorWidth.Value);
			Functions.WriteShort ((short)Window.VectorHeight.Value);
			Functions.WriteShort ((short)Window.FractWidth.Value);
			Functions.WriteShort ((short)Window.FractHeight.Value);
			Functions.WriteShort ((short)Window.ErrorWidth.Value);
			Functions.WriteShort ((short)Window.ErrorHeight.Value);
			Functions.WriteShort ((short)Window.IncrWidth.Value);
			Functions.WriteShort ((short)Window.IncrHeight.Value);
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

			Vars.LastOffset += Vars.SisNameSize; // Planet Name

			if (Vars.SaveVersion == 2 || Vars.SaveVersion == 4)
			{
				byte a = SummPtr.Activity;
				bool e = a == 3 || a == 6;

				if (e)
				{
					Functions.WriteByte ((byte)Window.difficultyBox.SelectedIndex);
					Functions.WriteByte (Convert.ToByte (Window.extendedCheckBox.Checked));
					Functions.WriteByte (Convert.ToByte (Window.nomadCheckBox.Checked));
					Functions.WriteInt ((int)Window.CustomSeed.Value);
				}
				else
				{
					Vars.LastOffset++; // Difficulty
					Vars.LastOffset++; // Extended
					Vars.LastOffset++; // Nomad
					Vars.LastOffset += 4; // Seed
				}
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

			Vars.LastOffset++; // Activity
			Vars.LastOffset++; // Flags
			Vars.LastOffset++; // day_index
			Vars.LastOffset++; // month_index
			Vars.LastOffset += 2; // year_index
			Vars.LastOffset += 2; // MCredit
			Vars.LastOffset++; // NumShips
			Vars.LastOffset++; // NumDevices
			Vars.LastOffset += 12; // ShipList
			Vars.LastOffset += 16; // DeviceList

			if (Vars.SaveVersion > 1)
			{
				if (Vars.SaveVersion != 3)
					Vars.LastOffset++; // res_factor

				Vars.LastOffset += (int)Vars.SaveNameLength; // SaveName
			}
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
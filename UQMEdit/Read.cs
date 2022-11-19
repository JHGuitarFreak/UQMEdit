using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UQMEdit.Load;

namespace UQMEdit
{
	partial class Read
	{
		public static FileStream Stream;
		public static Main Window;
		public static byte[] FileBuffer;
		public static byte SaveVersion = 0;
		public static bool FullyLoadSave = false;
		public static string SaveName;
		public static string Date;


		public static void Open(string FileName, Main window)
		{
			if (!File.Exists(FileName))
			{
				MessageBox.Show("Could not find path: " + FileName);
				return;
			}

			Stream = new FileStream(FileName, FileMode.Open);
			int FileSize = (int)Stream.Length;  // get file length
			FileBuffer = new byte[FileSize];    // create buffer
			Window = window;

			Vars.LastOffset = 0;
			FullyLoadSave = false;

			//// Save Checker
			uint LoadChecker = Functions.ReadUInt ();

			switch (LoadChecker)
			{
				case Vars.MMV3_TAG:
					SaveVersion = 4;
					break;
				case Vars.SAVEFILE_TAG:
					SaveVersion = 3;
					break;
				case Vars.MEGA_TAG:
					SaveVersion = 2;
					break;
				case Vars.SAVEFILE_TAG_HD:
					SaveVersion = 1;
					Vars.LastOffset += 12;
					break;
				default:
					SaveVersion = 0;
					Vars.LastOffset = 0;
					break;
			};

			if (SaveVersion > 1)
				FullyLoadSave = Modern.LoadGame ();
			else
				FullyLoadSave = Legacy.LoadGame ();

			if (FullyLoadSave)
			{
				// SIS_STATE

				Window.UniverseX.Value = Functions.LogXToUniverse (SSPtr.log_x) / 10;
				Window.UniverseY.Value = Functions.LogYToUniverse (SSPtr.log_y) / 10;

				decimal fuel = SSPtr.FuelOnBoard;
				fuel = fuel > 160100 ? 160100 : fuel;
				Window.ShipFuel.Value = fuel / 100;

				Window.ResUnits.Value = SSPtr.ResUnits;
				Window.ShipCrew.Value = SSPtr.CrewEnlisted;
				//Window.TotalMinerals.Value = SSPtr.TotalElementMass;
				Window.BioData.Value = SSPtr.TotalBioMass;

				{
					byte i = 0;
					foreach (object Modules in Window.ModulesBox.Controls)
					{
						if (Modules is ComboBox)
						{
							if (SSPtr.ModuleSlots[i] < 20)
								(Modules as ComboBox).SelectedIndex = SSPtr.ModuleSlots[i] - 2;
							else
								(Modules as ComboBox).SelectedIndex = 0;
							i++;
						}
					}

					i = 0;
					foreach (object Thrusters in Window.ThrusterBox.Controls)
					{
						if (Thrusters is CheckBox)
						{
							(Thrusters as CheckBox).Checked = SSPtr.DriveSlots[i] == 1 ? true : false;
							i++;
						}
					}

					i = 0;
					foreach (object Jets in Window.JetsBox.Controls)
					{
						if (Jets is CheckBox)
						{
							(Jets as CheckBox).Checked = SSPtr.JetSlots[i] == 2 ? true : false;
							i++;
						}
					}
				}

				Window.Landers.Value = SSPtr.NumLanders;

				Window.Common.Value = SSPtr.ElementAmounts[0];
				Window.Corrosive.Value = SSPtr.ElementAmounts[1];
				Window.BaseMetal.Value = SSPtr.ElementAmounts[2];
				Window.NobleGas.Value = SSPtr.ElementAmounts[3];
				Window.RareEarth.Value = SSPtr.ElementAmounts[4];
				Window.Precious.Value = SSPtr.ElementAmounts[5];
				Window.Radioactive.Value = SSPtr.ElementAmounts[6];
				Window.Exotic.Value = SSPtr.ElementAmounts[7];

				Window.ShipName.Text = SSPtr.ShipName;
				Window.CommanderName.Text = SSPtr.CommanderName;
				Window.NearestPlanet.Text = SSPtr.PlanetName;

				if (SaveVersion == 2 || SaveVersion == 4)
				{
					Window.difficultyBox.SelectedIndex = SSPtr.Difficulty;
					Window.extendedCheckBox.Checked = Convert.ToBoolean (SSPtr.Extended);
					Window.nomadCheckBox.Checked = Convert.ToBoolean (SSPtr.Nomad);
					Window.CustomSeed.Text = SSPtr.Seed.ToString ();

					if (Window.CustomSeed.Text == "0")
						Window.CustomSeed.Text = "16807";
				}

				// SummPtr

				{
					// Activity
					byte Activity = SummPtr.Activity;
					if (Activity < 0 || Activity >= Vars.StatusName.Length)
						Window.CurrentStatus.SelectedIndex = 9;
					else
						Window.CurrentStatus.SelectedIndex = Activity;

					// Planet Orbit
					if (!(Activity == 7 || Activity == 8))
						Window.NearestPlanet.Text = "Not In Orbit";
				}


				{
					// Flags
					bool FlagsBool (int OtherValue, bool Bomb = false)
					{
						if (!Bomb)
							return ((SummPtr.Flags | 128) & OtherValue) != 0 ? true : false;
						return (SummPtr.Flags & OtherValue) != 0 ? true : false;
					}
					Window.IsBomb.Checked = FlagsBool (128, true);
					Window.BioShield.Checked = FlagsBool (1);
					Window.QuakeShield.Checked = FlagsBool (2);
					Window.LightningShield.Checked = FlagsBool (4);
					Window.HeatShield.Checked = FlagsBool (8);
					Window.DblSpeed.Checked = FlagsBool (16);
					Window.DblCargo.Checked = FlagsBool (32);
					Window.RapidFire.Checked = FlagsBool (64);
				}

				// Superficial Date
				Date = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName (SummPtr.month_index).ToUpper () + " " + SummPtr.day_index + "·" + SummPtr.year_index;

				// Superficial Credits
				Window.Credits.Text = SummPtr.MCredit.ToString ();


				{
					// Superficial Escorts
					byte i = 0;
					foreach (object current in Window.ShipsBox.Controls)
					{
						if (current is ComboBox)
						{
							if (i < SummPtr.NumShips)
							{
								if (SummPtr.ShipList[i] < Vars.ShipNames.Length && SummPtr.ShipList[i] >= 0)
									(current as ComboBox).SelectedIndex = SummPtr.ShipList[i];
								else
									(current as ComboBox).SelectedIndex = 24;
								i++;
							}
							else
								(current as ComboBox).SelectedIndex = 24;
						}
					}
				}

				{
					// Superficial Devices
					object[] DevicesIntArray = new object[SummPtr.NumDevices];
					for (byte i = 0; i < SummPtr.NumDevices; i++)
					{
						if (SummPtr.DeviceList[i] < 0 || SummPtr.DeviceList[i] >= Vars.DeviceName.Length)
							DevicesIntArray[i] = "Please report this [0x" + SummPtr.DeviceList[i].ToString ("X2") + "]";
						else
							DevicesIntArray[i] = Vars.DeviceName[SummPtr.DeviceList[i]];
					}
					Window.Devices.Items.Clear ();
					Window.Devices.Items.AddRange (DevicesIntArray);
				}

				if (SaveVersion > 0)
					SaveName = SummPtr.SaveName;
				else
					SaveName = "Saved Game - Date: ";


				Console.WriteLine ("CrewCost = {0} FuelCost = {1}", GSPtr.CrewCost, GSPtr.FuelCost);

			}
			else
			{
				MessageBox.Show ("This is either not a UQM save or a corrupted UQM save!", "Important Message!");
				return;
			}

			Stream.Close();
			Stream.Dispose();
		}
	}
}
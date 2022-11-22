using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UQMEdit.Load;

namespace UQMEdit
{
	public class Read
	{
		public static FileStream Stream;
		public static Main Window;
		public static byte[] FileBuffer;
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

			Utilities.ResetAllControls (Window);
			Vars.clearVars ();

			FullyLoadSave = false;

			//// Save Checker
			uint LoadChecker = Functions.ReadUInt ();

			switch (LoadChecker)
			{
				case Vars.MMV3_TAG:
					Vars.SaveVersion = 4;
					break;
				case Vars.SAVEFILE_TAG:
					Vars.SaveVersion = 3;
					break;
				case Vars.MEGA_TAG:
					Vars.SaveVersion = 2;
					break;
				case Vars.SAVEFILE_TAG_HD:
					Vars.SaveVersion = 1;
					Vars.LastOffset += 12;
					break;
				default:
					Vars.SaveVersion = 0;
					Vars.LastOffset = 0;
					break;
			};

			if (Vars.SaveVersion > 1)
			{
				FullyLoadSave = Modern.LoadGame ();
				Window.Extras.Enabled = true;
				Window.Extras.Text = "Extras";
			}
			else
			{
				FullyLoadSave = Legacy.LoadGame ();
				Window.Extras.Enabled = false;
				Window.Extras.Text = "Extras (Disabled)";
			}

			if (FullyLoadSave)
			{
				// SIS_STATE

				Window.UniverseX.Value = (decimal)Functions.LogXToUniverse (SSPtr.log_x) / 10;
				Window.UniverseY.Value = (decimal)Functions.LogYToUniverse (SSPtr.log_y) / 10;

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

				Window.Common.Value      = SSPtr.ElementAmounts[0];
				Window.Corrosive.Value   = SSPtr.ElementAmounts[1];
				Window.BaseMetal.Value   = SSPtr.ElementAmounts[2];
				Window.NobleGas.Value    = SSPtr.ElementAmounts[3];
				Window.RareEarth.Value   = SSPtr.ElementAmounts[4];
				Window.Precious.Value    = SSPtr.ElementAmounts[5];
				Window.Radioactive.Value = SSPtr.ElementAmounts[6];
				Window.Exotic.Value      = SSPtr.ElementAmounts[7];

				Window.ShipName.Text = SSPtr.ShipName;
				Window.CommanderName.Text = SSPtr.CommanderName;
				Window.NearestPlanet.Text = SSPtr.PlanetName;

				if (Vars.SaveVersion == 2 || Vars.SaveVersion == 4)
				{
					Window.difficultyBox.SelectedIndex = SSPtr.Difficulty;
					Window.extendedCheckBox.Checked    = Convert.ToBoolean (SSPtr.Extended);
					Window.nomadCheckBox.Checked       = Convert.ToBoolean (SSPtr.Nomad);
					Window.CustomSeed.Text             = SSPtr.Seed.ToString ();

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
					Window.IsBomb.Checked          = FlagsBool (128, true);
					Window.BioShield.Checked       = FlagsBool (1);
					Window.QuakeShield.Checked     = FlagsBool (2);
					Window.LightningShield.Checked = FlagsBool (4);
					Window.HeatShield.Checked      = FlagsBool (8);
					Window.DblSpeed.Checked        = FlagsBool (16);
					Window.DblCargo.Checked        = FlagsBool (32);
					Window.RapidFire.Checked       = FlagsBool (64);
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

				if (Vars.SaveVersion > 0)
					SaveName = SummPtr.SaveName;
				else
					SaveName = "Saved Game - Date: ";

				// Game State

				if (Vars.SaveVersion > 1)
				{
					// Global Flags
					Window.ReadSpeed.Enabled = Vars.SaveVersion != 3 ? true : false;
					Window.ReadSpeed.Value        = GSPtr.glob_flags & Vars.READ_SPEED_MASK;
					// Window.CyborgCheckBox.Checked = Convert.ToBoolean (GSPtr.glob_flags & Vars.CYBORG_ENABLED);
					Window.CombatSpeed.Value      = (GSPtr.glob_flags & Vars.COMBAT_SPEED_MASK) >> Vars.COMBAT_SPEED_SHIFT;
					Window.MusicCheckBox.Checked  = Convert.ToBoolean (GSPtr.glob_flags & Vars.MUSIC_DISABLED);
					Window.SoundCheckBox.Checked  = Convert.ToBoolean (GSPtr.glob_flags & Vars.SOUND_DISABLED);

					// Costs

					Window.CrewCost.Value           = GSPtr.CrewCost;
					if (Vars.SaveVersion == 3)
					{
						Window.FuelCost.ReadOnly = false;
						Window.FuelCost.Increment = 1;
					}
					else
					{
						Window.FuelCost.ReadOnly = true;
						Window.FuelCost.Increment = 0;
					}
					Window.FuelCost.Value           = GSPtr.FuelCost;
					Window.LanderCost.Value         = GSPtr.ModuleCost[0]  * 50;
					Window.ThrusterCost.Value       = GSPtr.ModuleCost[1]  * 50;
					Window.JetCost.Value            = GSPtr.ModuleCost[2]  * 50;
					Window.CrewPodCost.Value        = GSPtr.ModuleCost[3]  * 50;
					Window.StorageBayCost.Value     = GSPtr.ModuleCost[4]  * 50;
					Window.FuelTankCost.Value       = GSPtr.ModuleCost[5]  * 50;
					Window.HiEffFuelSysCost.Value   = GSPtr.ModuleCost[6]  * 50;
					Window.DynamoUnitCost.Value     = GSPtr.ModuleCost[7]  * 50;
					Window.ShivaFurnaceCost.Value   = GSPtr.ModuleCost[8]  * 50;
					Window.IonBoltGunCost.Value     = GSPtr.ModuleCost[9]  * 50;
					Window.FusionBlasterCost.Value  = GSPtr.ModuleCost[10] * 50;
					Window.HellboreCannonCost.Value = GSPtr.ModuleCost[11] * 50;
					Window.TrackingSystemCost.Value = GSPtr.ModuleCost[12] * 50;
					Window.PointDefenseCost.Value   = GSPtr.ModuleCost[13] * 50;

					Window.CommonWorth.Value        = GSPtr.ElementWorth[0];
					Window.CorrosiveWorth.Value     = GSPtr.ElementWorth[1];
					Window.BaseMetalWorth.Value     = GSPtr.ElementWorth[2];
					Window.NobleGasWorth.Value      = GSPtr.ElementWorth[3];
					Window.RareEarthWorth.Value     = GSPtr.ElementWorth[4];
					Window.PreciousMetalWorth.Value = GSPtr.ElementWorth[5];
					Window.RadioactiveWorth.Value   = GSPtr.ElementWorth[6];
					Window.ExoticWorth.Value        = GSPtr.ElementWorth[7];

					// Clock State
					
					Window.DayIndex.Value   = ClockPtr.day_index;
					Window.MonthIndex.Value = ClockPtr.month_index;
					Window.YearIndex.Value  = ClockPtr.year_index;
					Window.TickCount.Value  = ClockPtr.tick_count;
					Window.DayInTicks.Value = ClockPtr.day_in_ticks;

					// Ship Current Location

					Window.AutoPilotX.Value  = GSPtr.autopilot_x;
					Window.AutoPilotY.Value  = GSPtr.autopilot_y;
					Window.IPLocationX.Value = GSPtr.ip_location_x;
					Window.IPLocationY.Value = GSPtr.ip_location_y;
					Window.ShipOriginX.Value = GSPtr.ShipStamp_x;
					Window.ShipOriginY.Value = GSPtr.ShipStamp_y;
					Window.ShipFacing.Value  = GSPtr.ShipFacing;

					// Ship Velocity

					Window.TravelAngle.Value  = GSPtr.TravelAngle;
					Window.VectorWidth.Value  = GSPtr.vector_width;
					Window.VectorHeight.Value = GSPtr.vector_height;
					Window.FractWidth.Value   = GSPtr.fract_width;
					Window.FractHeight.Value  = GSPtr.fract_height;
					Window.ErrorWidth.Value   = GSPtr.error_width;
					Window.ErrorHeight.Value  = GSPtr.error_height;
					Window.IncrWidth.Value    = GSPtr.incr_width;
					Window.IncrHeight.Value   = GSPtr.incr_height;
				}
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
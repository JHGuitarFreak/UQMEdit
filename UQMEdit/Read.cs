using System;
using System.IO;
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
			uint LoadChecker = Functions.ReadUInt (Vars.LastOffset);

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
					Vars.LastOffset = 1;
					break;
			};

			Console.WriteLine ("SaveVersion = {0} LastOffset = {1}", SaveVersion, Vars.LastOffset);

			if (SaveVersion > 1)
				FullyLoadSave = Modern.LoadGame ();
			else
				FullyLoadSave = Legacy.LoadGame ();

			if (FullyLoadSave)
			{
				Window.UniverseX.Value = Functions.LogXToUniverse (SSPtr.log_x) / 10;
				Window.UniverseY.Value = Functions.LogYToUniverse (SSPtr.log_y) / 10;

				decimal fuel = SSPtr.FuelOnBoard;
				fuel = fuel > 160100 ? 160100 : fuel;
				Window.ShipFuel.Value = fuel / 100;

				Window.ResUnits.Value = SSPtr.ResUnits;
				Window.ShipCrew.Value = SSPtr.CrewEnlisted;
				//Window.TotalMinerals.Value = SSPtr.TotalElementMass;
				Window.BioData.Value = SSPtr.TotalBioMass;

				//ModuleSlots
				//ThrusterArray
				//JetSlots

				Window.Landers.Value = SSPtr.NumLanders;

				Window.Common.Value =      SSPtr.ElementAmounts[0];
				Window.Corrosive.Value =   SSPtr.ElementAmounts[1];
				Window.BaseMetal.Value =   SSPtr.ElementAmounts[2];
				Window.NobleGas.Value =    SSPtr.ElementAmounts[3];
				Window.RareEarth.Value =   SSPtr.ElementAmounts[4];
				Window.Precious.Value =    SSPtr.ElementAmounts[5];
				Window.Radioactive.Value = SSPtr.ElementAmounts[6];
				Window.Exotic.Value =      SSPtr.ElementAmounts[7];

				Window.ShipName.Text = SSPtr.ShipName;
				Window.CommanderName.Text = SSPtr.CommanderName;
				Window.NearestPlanet.Text = SSPtr.PlanetName;

				if (SaveVersion == 2 || SaveVersion == 4)
				{
					Window.difficultyBox.SelectedIndex = SSPtr.Difficulty;
					Window.extendedCheckBox.Checked = Convert.ToBoolean (SSPtr.Extended);
					Window.nomadCheckBox.Checked = Convert.ToBoolean (SSPtr.Nomad);
					Window.CustomSeed.Text = SSPtr.Seed.ToString ();
				}
			}
			else
				return;

			Stream.Close();
			Stream.Dispose();
		}
	}
}
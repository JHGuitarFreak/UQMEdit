using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UQMEdit
{
	public partial class Main : Form
	{
		private string CurrentDir;
		private string CurrentFile = "";
		public static string FileName = "";
		public static object[] ShipModules;

		public Main() {
			InitializeComponent();
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			ShipModules = Modules.CreateModules();

			foreach (object ships in ShipsBox.Controls) {
				if (ships is ComboBox) {
					(ships as ComboBox).Items.AddRange(Vars.ShipNames);
				}
			}
			foreach (object current in ModulesBox.Controls) {
				if (current is ComboBox) {
					(current as ComboBox).Items.AddRange(ShipModules);
				}
			}
			CurrentStatus.Items.AddRange(Vars.StatusName);
			difficultyBox.Items.AddRange(Vars.Difficulties);
			Spoilers.Checked = false;

			string PathVanilla, PathHD, PathMegaMod, PathRemix, PathDesired;
			string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			PathVanilla = PathAppData + "\\uqm\\save";
			PathHD = PathAppData + "\\uqmhd\\save";
			PathRemix = PathAppData + "\\uqmhdremix\\save";
			PathMegaMod = PathAppData + "\\UQM-MegaMod\\save";

			if (Directory.Exists(PathMegaMod))
				PathDesired = PathMegaMod;
			else if (Directory.Exists(PathRemix))
				PathDesired = PathRemix;
			else if (Directory.Exists(PathHD))
				PathDesired = PathHD;
			else if (Directory.Exists(PathVanilla))
				PathDesired = PathVanilla;
			else
				PathDesired = Directory.GetCurrentDirectory();

			CurrentDir = PathDesired;
			StarList.Items.AddRange(ParseStars.LoadStars(false));
		}

		private void Open_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Open UQM Save File";
			openFileDialog.Filter = "UQM Save Files | starcon2.*; uqmsave.*";
			openFileDialog.InitialDirectory = CurrentDir;

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				CurrentFile = openFileDialog.FileName;
				FileName = Path.GetFileName(CurrentFile);
				Reload.Enabled = true;
				Save.Enabled = true;
				Tabs.Enabled = true;

				Read.Open(CurrentFile, this);

				string TitleText = "The UQM Save Editor";
				SeedBox.Visible = false;
				megaModModes.Visible = false;
				switch (Vars.SaveVersion)
				{
					case 4:
						TitleText += ": MegaMod v0.8.2 - ";
						SeedBox.Visible = true;
						megaModModes.Visible = true;
						break;
					case 3:
						TitleText += ": Core v0.8.0 - ";
						break;
					case 2:
						TitleText += ": Legacy MegaMod - ";
						SeedBox.Visible = true;
						megaModModes.Visible = true;
						break;
					case 1:
						TitleText += ": HD-mod v0.7.0 - ";
						break;
					case 0:
					default:
						TitleText += ": Core v0.7.0 - ";
						break;
				}
				Text = TitleText + (Vars.SaveVersion > 0 ? (Read.Date + ": " + Read.SaveName) : (Read.SaveName + Read.Date));

				CurrentDir = CurrentFile;
			}
		}

		private void Reload_Click(object sender, EventArgs e) {
			Read.Open(CurrentFile, this);
		}

		private void MineralsValueChanged(object sender, EventArgs e) {
			TotalMinerals.Value = 0;
			TotalMinerals.Value = Common.Value + Corrosive.Value +
										BaseMetal.Value + NobleGas.Value +
										RareEarth.Value + Precious.Value +
										Radioactive.Value + Exotic.Value;
		}

		private decimal nearestFifty (decimal value)
		{
			if (((int)value % 50) != 0)
				return Math.Round(value / 50) * 50;
			else
				return value;
		}

		private void LanderCostChanged (object sender, EventArgs e)
		{
			LanderCost.Value = nearestFifty (LanderCost.Value);
		}

		private void ThrusterCostChanged (object sender, EventArgs e)
		{
			ThrusterCost.Value = nearestFifty (ThrusterCost.Value);
		}

		private void JetCostChanged (object sender, EventArgs e)
		{
			JetCost.Value = nearestFifty (JetCost.Value);
		}

		private void CrewPodCostChanged (object sender, EventArgs e)
		{
			CrewPodCost.Value = nearestFifty (CrewPodCost.Value);
		}

		private void StorageBayCostChanged (object sender, EventArgs e)
		{
			StorageBayCost.Value = nearestFifty (StorageBayCost.Value);
		}

		private void FuelTankCostChanged (object sender, EventArgs e)
		{
			FuelTankCost.Value = nearestFifty (FuelTankCost.Value);
		}

		private void HiEffFuelSysCostChanged (object sender, EventArgs e)
		{
			HiEffFuelSysCost.Value = nearestFifty (HiEffFuelSysCost.Value);
		}

		private void DynamoUnitCostChanged (object sender, EventArgs e)
		{
			DynamoUnitCost.Value = nearestFifty (DynamoUnitCost.Value);
		}

		private void ShivaFurnaceCostChanged (object sender, EventArgs e)
		{
			ShivaFurnaceCost.Value = nearestFifty (ShivaFurnaceCost.Value);
		}

		private void IonBoltGunCostChanged (object sender, EventArgs e)
		{
			IonBoltGunCost.Value = nearestFifty (IonBoltGunCost.Value);
		}

		private void FusionBlasterCostChanged (object sender, EventArgs e)
		{
			FusionBlasterCost.Value = nearestFifty (FusionBlasterCost.Value);
		}

		private void HellboreCannonCostChanged (object sender, EventArgs e)
		{
			HellboreCannonCost.Value = nearestFifty (HellboreCannonCost.Value);
		}

		private void TrackingSystemCostChanged (object sender, EventArgs e)
		{
			TrackingSystemCost.Value = nearestFifty (TrackingSystemCost.Value);
		}

		private void PointDefenseCostChanged (object sender, EventArgs e)
		{
			PointDefenseCost.Value = nearestFifty (PointDefenseCost.Value);
		}

		private void Save_Click(object sender, EventArgs e) {
			Write.Save(CurrentFile, this);
		}

		private void Main_Shown(object sender, EventArgs e) {
			UniverseY.Text = "";
			UniverseX.Text = "";
			NearestStar.Text = "";
		}

		private void Spoilers_CheckedChanged(object sender, EventArgs e) {
			int selectedIndex = StarList.SelectedIndex;
			StarList.Items.Clear();
			StarList.Items.AddRange(ParseStars.LoadStars(Spoilers.Checked));
			if (StarList.Items.Count >= selectedIndex) {
				if (StarList.Items.Count >= selectedIndex + 23) {
					StarList.SelectedIndex = selectedIndex + 23;
				}
				StarList.SelectedIndex = selectedIndex;
			}
		}

		private void Universe_TextChanged(object sender, EventArgs e) {
			double num, num2;
			if (double.TryParse(UniverseX.Text.Replace(',', '.'), out num) && double.TryParse(UniverseY.Text.Replace(',', '.'), out num2)) {
				if (num <= 1500.0 && num2 <= 1500.0 && num >= -1500.0 && num2 >= -1500.0) {
					NearestStar.Text = Stars.NearestStar(num, num2);
					return;
				}
				NearestStar.Text = "";
			}
		}

		private void StarList_SelectedIndexChanged(object sender, EventArgs e) {
			if (StarList.SelectedItem == null || StarList.SelectedIndex == 0) {
				return;
			}
			string[] array = StarList.SelectedItem.ToString().Split(new char[]
			{
				'\t'
			})[4].Replace(" ", "").Replace("[", "").Replace("]", "").Split(new char[]
			{
				':'
			});
			UniverseX.Text = array[0];
			UniverseY.Text = array[1];
		}

		private void UpgradeToMax_Click(object sender, EventArgs e) {
			byte[] ModulesArray = { 4, 1, 1, 2, 11, 10, 10, 10, 5, 5, 5, 6, 6, 6, 9, 9 };
			byte[] ModulesArrayBomb = { 16, 17, 15, 13, 12, 13, 15, 16, 17, 14, 4, 1, 11, 10, 9, 10 };
			byte i = 0;
			foreach (object Modules in ModulesBox.Controls) {
				if (Modules is ComboBox) {
					(Modules as ComboBox).SelectedIndex = IsBomb.Checked ? ModulesArrayBomb[i] : ModulesArray[i];
					i++;
				}
			}
		}

		private void MaxThrusters_Click(object sender, EventArgs e) {
			foreach (object Thruster in ThrusterBox.Controls) {
				if (Thruster is CheckBox) {
					(Thruster as CheckBox).Checked = true;
				}
			}
		}

		private void MaxJets_Click(object sender, EventArgs e) {
			foreach (object Jets in JetsBox.Controls) {
				if (Jets is CheckBox) {
					(Jets as CheckBox).Checked = true;
				}
			}
		}

		private void Module_SelectedIndexChanged(object sender, EventArgs e) {
			int MaxStorage = 0, MaxFuel = 10, MaxCrew = 0;
			foreach (object Module in ModulesBox.Controls) {
				if (Module is ComboBox) {
					int Index = (Module as ComboBox).SelectedIndex;
					MaxCrew += Index == 1 ? 50 : 0;
					MaxStorage += Index == 2 ? 500 : 0;
					MaxFuel += Index == 3 ? 50 : (Index == 4 ? 100 : 0);
				}
			}
			CrewLabel.Text = "Crew " + ("[" + MaxCrew + "]");
			FuelLabel.Text = "Fuel  " + ("[" + MaxFuel + "]");
			TotalLabel.Text = "Total  " + ("[" + MaxStorage + "]");
			MaxLimits.SetToolTip(CrewLabel, "Please fill only to max value as shown.");
			MaxLimits.SetToolTip(FuelLabel, "Please fill only to max value as shown.");
			MaxLimits.SetToolTip(TotalLabel, "Please fill only to max value as shown.");
			// These are commented out because they sometimes cause errors when loading.
			//ShipFuel.Maximum = MaxFuel;
			//ShipCrew.Maximum = MaxCrew;
		}

		private void ReadSpeedChanged (object sender, EventArgs e)
		{
			switch (ReadSpeed.Value)
			{
				case 4:
					ReadSpeedLabel.Text = "Very Fast";
					break;
				case 3:
					ReadSpeedLabel.Text = "Fast";
					break;
				case 2:
					ReadSpeedLabel.Text = "Moderate";
					break;
				case 1:
					ReadSpeedLabel.Text = "Slow";
					break;
				case 0:
				default:
					ReadSpeedLabel.Text = "Very Slow";
					break;
			}
		}

		private void CombatSpeedChanged (object sender, EventArgs e)
		{
			switch (CombatSpeed.Value)
			{
				case 3:
					CombatSpeedLabel.Text = "Cyborg++";
					break;
				case 2:
					CombatSpeedLabel.Text = "Cyborg+";
					break;
				case 1:
					CombatSpeedLabel.Text = "Cyborg";
					break;
				case 0:
				default:
					CombatSpeedLabel.Text = "Disabled";
					break;
			}
		}
	}
}
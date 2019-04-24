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
		private object[] ShipModules;

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
			Spoilers.Checked = false;

			string PathVanilla, PathHD, PathMegaMod, PathRemix, PathDesired;
			string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			PathVanilla = PathAppData + "\\uqm\\save";
			PathHD = PathAppData + "\\uqmhd\\save";
			PathRemix = PathAppData + "\\uqmhdremix\\save";
			PathMegaMod = PathAppData + "\\UQM-MegaMod\\save";

			if (Directory.Exists(PathVanilla)) {
				PathDesired = PathVanilla;
			} else if (Directory.Exists(PathHD)) {
				PathDesired = PathHD;
			} else if (Directory.Exists(PathRemix)) {
				PathDesired = PathRemix;
			} else if (Directory.Exists(PathMegaMod)) {
				PathDesired = PathMegaMod;
			} else {
				PathDesired = Directory.GetCurrentDirectory();
			}

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
				switch (Read.SaveVersion) {
					case 3:
						TitleText += ": macOS - ";
						break;
					case 2:
						TitleText += ": MegaMod - ";
						SeedBox.Visible = true;
						break;
					case 1:
						TitleText += ": HD-mod - ";
						break;
					case 0:
						TitleText += ": Vanilla - ";
						break;
					default:
						break;
				}
				Text = TitleText + (Read.SaveVersion > 0 ? (Read.Date + ": " + Read.SaveName) : Read.Date);

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

		private void Save_Click(object sender, EventArgs e) {
			Write.Save(CurrentFile, this);
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
	}
}

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
		private object[] shipModules;

		public Main() {
			InitializeComponent();
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			this.shipModules = Modules.CreateModules();

			foreach (object current4 in this.ShipsBox.Controls) {
				if (current4 is ComboBox) {
					(current4 as ComboBox).Items.AddRange(Vars.escortNames);
				}
			}
			this.CurrentStatus.Items.AddRange(Vars.statusName);
			this.Spoilers.Checked = false;

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
			this.StarList.Items.AddRange(ParseStars.LoadStars(false));
		}

		private void Open_Click(object sender, EventArgs e) {
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Title = "Open Save File";
			openFileDialog.Filter = "Save File (*.*)|*.*";
			openFileDialog.InitialDirectory = CurrentDir;

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				CurrentFile = openFileDialog.FileName;
				FileName = Path.GetFileName(CurrentFile);
				this.Reload.Enabled = true;
				this.Save.Enabled = true;
				this.Tabs.Enabled = true;

				Read.Open(CurrentFile, this);

				string TitleText = "The UQM Save Editor";
				this.SeedBox.Visible = false;
				switch (Read.SaveVersion) {
					case 3:
						TitleText += ": macOS - ";
						break;
					case 2:
						TitleText += ": MegaMod - ";
						this.SeedBox.Visible = true;
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
				this.Text = TitleText + (Read.SaveVersion > 0 ? (Read.TimeDate + " - " + Read.SaveName) : Read.TimeDate);

				CurrentDir = CurrentFile;
			}
		}

		private void Reload_Click(object sender, EventArgs e) {
			Read.Open(CurrentFile, this);
		}

		private void Save_Click(object sender, EventArgs e) {
			Write.Save(CurrentFile, this);
		}
	}
}

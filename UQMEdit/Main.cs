using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UQMEdit
{
	public partial class Main : Form
	{
		private string CurrentDir;
		private string CurrentFile = "";
		public static string FileName = "";

		public Main() {
			InitializeComponent();
		}

		private void Open_Click(object sender, EventArgs e) {
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			CurrentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			openFileDialog.Title = "Open Save File";
			openFileDialog.Filter = "Save File (*.*)|*.*";
			openFileDialog.InitialDirectory = CurrentDir;

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				CurrentFile = openFileDialog.FileName;
				FileName = Path.GetFileName(CurrentFile);
				this.Text = "The UQM Save Editor - " + FileName;
				this.Reload.Enabled = true;
				this.Save.Enabled = true;
				this.Tabs.Enabled = true;

				//Read.Open(CurrentFile, this);
				CurrentDir = CurrentFile;
			}
		}

		private void Reload_Click(object sender, EventArgs e) {
			//Read.Open(CurrentFile, this);
		}

		private void Save_Click(object sender, EventArgs e) {
			//Write.Save(CurrentFile, this);
		}
	}
}

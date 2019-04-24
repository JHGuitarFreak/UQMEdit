using System;
using System.IO;
using System.Windows.Forms;

namespace UQMEdit
{
	partial class Read
	{


		public static void ShipStatus() {
			byte[] ShipsArray = new byte[12];

			//  Escorts
			Read.Stream.Seek(Functions.OffsPick(Offs.HD.Escorts[0], Offs.MM.Escorts[0]), SeekOrigin.Begin);
			Read.Stream.Read(ShipsArray, 0, 1);
			int num4 = (int)ShipsArray[0];
			Read.Stream.Seek(Functions.OffsPick(Offs.HD.Escorts[1], Offs.MM.Escorts[1]), SeekOrigin.Begin);
			Read.Stream.Read(ShipsArray, 0, num4);
			int num5 = 0;
			foreach (object current in Window.ShipsBox.Controls) {
				if (current is ComboBox) {
					if (num5 < num4) {
						if ((int)ShipsArray[num5] < Vars.ShipNames.Length && ShipsArray[num5] >= 0) {
							(current as ComboBox).SelectedIndex = (int)ShipsArray[num5];
						} else {
							(current as ComboBox).SelectedIndex = 24;
						}
						num5++;
					} else {
						(current as ComboBox).SelectedIndex = 24;
					}
				}
			}

			// Modules
			byte[] ModulesArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.ModuleSlots, Offs.MM.ModuleSlots), 16);
			int MDCount = 0;
			foreach (object Modules in Window.ModulesBox.Controls) {
				if (Modules is ComboBox) {
					if (ModulesArray[MDCount] == 22) {
						(Modules as ComboBox).SelectedIndex = 0;
					} else {
						if (ModulesArray[MDCount] <= 20) {
							(Modules as ComboBox).SelectedIndex = ModulesArray[MDCount]-2;
						} else {
							(Modules as ComboBox).SelectedIndex = 0;
						}
					}
					MDCount++;
				}
			}

			// Anti-Mat Thrusters
			Window.Thruster0.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[0], Offs.MM.DriveSlots[0]));
			Window.Thruster1.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[1], Offs.MM.DriveSlots[1]));
			Window.Thruster2.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[2], Offs.MM.DriveSlots[2]));
			Window.Thruster3.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[3], Offs.MM.DriveSlots[3]));
			Window.Thruster4.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[4], Offs.MM.DriveSlots[4]));
			Window.Thruster5.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[5], Offs.MM.DriveSlots[5]));
			Window.Thruster6.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[6], Offs.MM.DriveSlots[6]));
			Window.Thruster7.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[7], Offs.MM.DriveSlots[7]));
			Window.Thruster8.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[8], Offs.MM.DriveSlots[8]));
			Window.Thruster9.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[9], Offs.MM.DriveSlots[9]));
			Window.Thruster10.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.DriveSlots[10], Offs.MM.DriveSlots[10]));

			// Turning Jets
			Window.Jets0.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[0], Offs.MM.JetSlots[0]), true);
			Window.Jets1.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[1], Offs.MM.JetSlots[1]), true);
			Window.Jets2.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[2], Offs.MM.JetSlots[2]), true);
			Window.Jets3.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[3], Offs.MM.JetSlots[3]), true);
			Window.Jets4.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[4], Offs.MM.JetSlots[4]), true);
			Window.Jets5.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[5], Offs.MM.JetSlots[5]), true);
			Window.Jets6.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[6], Offs.MM.JetSlots[6]), true);
			Window.Jets7.Checked = Functions.ReadOffsetBool(Functions.OffsPick(Offs.HD.JetSlots[7], Offs.MM.JetSlots[7]), true);
		}
	}
}

using System;
using System.IO;
using System.Windows.Forms;

namespace UQMEdit
{
	partial class Read
	{
		public static void ShipStatus() {

			//  Escorts
			byte NumberOfShips = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Escorts[0], Offs.MM.Escorts[0]), 1)[0];
			byte[] ShipsArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Escorts[1], Offs.MM.Escorts[1]), NumberOfShips);
			byte ShipCount = 0;
			foreach (object current in Window.ShipsBox.Controls) {
				if (current is ComboBox) {
					if (ShipCount < NumberOfShips) {
						if (ShipsArray[ShipCount] < Vars.ShipNames.Length && ShipsArray[ShipCount] >= 0) {
							(current as ComboBox).SelectedIndex = ShipsArray[ShipCount];
						} else {
							(current as ComboBox).SelectedIndex = 24;
						}
						ShipCount++;
					} else {
						(current as ComboBox).SelectedIndex = 24;
					}
				}
			}

			// Anti-Mat Thrusters
			byte[] ThrustersArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.DriveSlots[0], Offs.MM.DriveSlots[0]), 11);
			Array.Reverse(ThrustersArray, 0, 11);
			byte TCount = 0;
			foreach (object Thrusters in Window.ThrusterBox.Controls) {
				if (Thrusters is CheckBox) {
					(Thrusters as CheckBox).Checked = ThrustersArray[TCount] == 1 ? true : false;
					TCount++;
				}
			}

			// Turning Jets
			byte[] JetsArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.JetSlots[0], Offs.MM.JetSlots[0]), 8);
			Array.Reverse(JetsArray, 0, 8);
			byte JCount = 0;
			foreach (object Jets in Window.JetsBox.Controls) {
				if (Jets is CheckBox) {
					(Jets as CheckBox).Checked = JetsArray[JCount] == 2 ? true : false;
					JCount++;
				}
			}

			// Modules
			byte[] ModulesArray = Functions.ReadOffset(Functions.OffsPick(Offs.HD.ModuleSlots, Offs.MM.ModuleSlots), 16);
			byte MDCount = 0;
			foreach (object Modules in Window.ModulesBox.Controls) {
				if (Modules is ComboBox) {
					if (ModulesArray[MDCount] < 20) {
						(Modules as ComboBox).SelectedIndex = ModulesArray[MDCount] - 2;
					} else {
						(Modules as ComboBox).SelectedIndex = 0;
					}
					MDCount++;
				}
			}

		}
	}
}

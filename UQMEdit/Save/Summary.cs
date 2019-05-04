using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UQMEdit
{
	partial class Write
	{
		public static void Summary() {
			// Resource Units
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.ResUnits, Offs.MM.ResUnits), Window.ResUnits.Value, 4, 0xFFFFFFFF, true);
			// Fuel
			decimal ShipFuel = Window.ShipFuel.Value * 100;
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Fuel, Offs.MM.Fuel), Decimal.ToUInt32(ShipFuel), 4, 161000, true);
			// Crew
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.SiSCrew, Offs.MM.SiSCrew), Window.ShipCrew.Value, 2, 800, true);
			// Total Minerals
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.TotalMinerals, Offs.MM.TotalMinerals), Window.TotalMinerals.Value, 2, 8000, true);
			// Bio Data
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.BioData, Offs.MM.BioData), Window.BioData.Value, 2, 0xFFFF, true);

			// Modules
			byte Mods, i = 0;
			foreach (object Mod in Window.ModulesBox.Controls) {
				if (Mod is ComboBox) {
					int Index = (Mod as ComboBox).SelectedIndex;
					if (Index > 0) {
						Mods = (byte)(Index + 2);
					} else {
						Mods = 22;
					}
					Functions.WriteOffset(Functions.OffsPick(Offs.HD.ModuleSlots + i, Offs.MM.ModuleSlots + i), Mods, 1, 22, true);
					i++;
				}
			}

			// Thrusters
			byte Thrusters, j = 0;
			foreach (object Thruster in Window.ThrusterBox.Controls) {
				if (Thruster is CheckBox) {
					bool Index = (Thruster as CheckBox).Checked;
					Thrusters = (byte)(Index ? 1 : 20);
					Functions.WriteOffset(Functions.OffsPick(Offs.HD.DriveSlots[j], Offs.MM.DriveSlots[j]), Thrusters, 1, 20, true);
					j++;
				}
			}

			// Thrusters
			byte Jets, k = 0;
			foreach (object Jet in Window.JetsBox.Controls) {
				if (Jet is CheckBox) {
					bool Index = (Jet as CheckBox).Checked;
					Jets = (byte)(Index ? 2 : 21);
					Functions.WriteOffset(Functions.OffsPick(Offs.HD.JetSlots[k], Offs.MM.JetSlots[k]), Jets, 1, 21, true);
					k++;
				}
			}

			// Landers
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Landers, Offs.MM.Landers), Window.Landers.Value, 1, 10);

			// Cargo
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Common, Offs.MM.Common), Window.Common.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Corrosive, Offs.MM.Corrosive), Window.Corrosive.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.BaseMetal, Offs.MM.BaseMetal), Window.BaseMetal.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.NobleGas, Offs.MM.NobleGas), Window.NobleGas.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.RareEarth, Offs.MM.RareEarth), Window.RareEarth.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Precious, Offs.MM.Precious), Window.Precious.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Radioactive, Offs.MM.Radioactive), Window.Radioactive.Value, 2, 0xFFFF);
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.Exotic, Offs.MM.Exotic), Window.Exotic.Value, 2, 0xFFFF);

			// Ship Name
			Functions.WriteOffsetString(Functions.OffsPick(Offs.HD.ShipName, Offs.MM.ShipName), Window.ShipName.Text, 16);
			// Captain Name
			Functions.WriteOffsetString(Functions.OffsPick(Offs.HD.CaptainName, Offs.MM.CaptainName), Window.CommanderName.Text, 16);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UQMEdit
{
	partial class Read
	{
		public static void Coordinates() {
			// X Coordinates
			decimal LogX = Functions.LogXToUniverse(Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.LogX, Offs.MM.LogX), 4));
			Window.UniverseX.Value = LogX / 10;
			// Y Coordinates
			decimal LogY = Functions.LogYToUniverse(Functions.ReadOffsetToInt(Functions.OffsPick(Offs.HD.LogY, Offs.MM.LogY), 4));
			Window.UniverseY.Value = LogY / 10;

			// Status
			byte Status = Functions.ReadOffset(Functions.OffsPick(Offs.HD.Status, Offs.MM.Status, Offs.Core.Status), 1)[0];
			if (Status < 0 || Status >= Vars.StatusName.Length) {
				Window.CurrentStatus.SelectedIndex = 9;
			} else {
				Window.CurrentStatus.SelectedIndex = Status;
			}

			// Planet Orbit
			if (Status == 7 || Status == 8) {
				string Planet = Encoding.Default.GetString(Functions.ReadOffset(Functions.OffsPick(Offs.HD.NearestPlanet, Offs.MM.NearestPlanet), 16));
				Window.NearestPlanet.Text = Planet;
			} else {
				Window.NearestPlanet.Text = "Not In Orbit";
			}
		}
	}
}

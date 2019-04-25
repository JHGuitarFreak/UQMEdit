using System;
using System.Windows.Forms;

namespace UQMEdit
{
	partial class Write
	{
		public static void Coordinates() {
			int snum;

			// UniverseX
			decimal UniverseX = Window.UniverseX.Value * 10;
			snum = Functions.UniverseToLogX(decimal.ToInt32(UniverseX));
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.LogX, Offs.MM.LogX), snum, 4, 159735);

			// UniverseY
			decimal UniverseY = Window.UniverseY.Value * 10;
			snum = Functions.UniverseToLogY(decimal.ToInt32(UniverseY));
			Functions.WriteOffset(Functions.OffsPick(Offs.HD.LogY, Offs.MM.LogY), snum, 4, 191990);
		}
	}
}
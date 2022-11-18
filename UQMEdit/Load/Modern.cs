using System;

namespace UQMEdit.Load
{
	partial class Modern
	{
		public static bool LoadSisState ()
		{
			int SisNameSize = Read.SaveVersion == 4 ? 32 : 16;

			SSPtr.log_x = Functions.ReadInt (Vars.LastOffset);
			SSPtr.log_y = Functions.ReadInt (Vars.LastOffset);
			SSPtr.ResUnits = Functions.ReadUInt (Vars.LastOffset);
			SSPtr.FuelOnBoard = Functions.ReadUInt (Vars.LastOffset);
			SSPtr.CrewEnlisted = Functions.ReadUShort (Vars.LastOffset);
			SSPtr.TotalElementMass = Functions.ReadUShort (Vars.LastOffset);
			SSPtr.TotalBioMass = Functions.ReadUShort (Vars.LastOffset);
			SSPtr.ModuleSlots = Functions.ReadByteArray (Vars.LastOffset, 16);
			SSPtr.DriveSlots = Functions.ReadByteArray (Vars.LastOffset, 11);
			SSPtr.JetSlots = Functions.ReadByteArray (Vars.LastOffset, 8);
			SSPtr.NumLanders = Functions.ReadByte (Vars.LastOffset);

			for (int i = 0; i < 8; i++)
				SSPtr.ElementAmounts[i] = Functions.ReadUShort (Vars.LastOffset);

			SSPtr.ShipName = Functions.ReadStr (Vars.LastOffset, SisNameSize);
			SSPtr.CommanderName = Functions.ReadStr (Vars.LastOffset, SisNameSize);
			SSPtr.PlanetName = Functions.ReadStr (Vars.LastOffset, SisNameSize);

			if (Read.SaveVersion == 2 || Read.SaveVersion == 4)
			{
				SSPtr.Difficulty = Functions.ReadByte (Vars.LastOffset);
				SSPtr.Extended = Functions.ReadByte (Vars.LastOffset);
				SSPtr.Nomad = Functions.ReadByte (Vars.LastOffset);
				SSPtr.Seed = Functions.ReadInt (Vars.LastOffset);
			}

			return true;
		}

		public static bool LoadSummary ()
		{
			uint magic;
			uint nameSize = 0;

			magic = Functions.ReadUInt (Vars.LastOffset);
			if (magic != Vars.SUMMARY_TAG)
				return false;

			magic = Functions.ReadUInt (Vars.LastOffset);
			if (magic < 160)
				return false;

			nameSize = magic - 160;

			Console.WriteLine ("magic = {0} nameSize = {1}", magic, nameSize);

			if (!LoadSisState ())
				return false;

			return true;
		}

		public static bool LoadGame ()
		{
			if (!LoadSummary ())
			{
				System.Windows.Forms.MessageBox.Show ("Not a proper UQM or MegaMod save!");
				return false;
			}

			return true;
		}
	}
}

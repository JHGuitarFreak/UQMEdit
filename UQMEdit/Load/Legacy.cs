﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UQMEdit.Load
{
	partial class Legacy
	{
		public static void LoadClockState ()
		{
			ClockPtr.day_index = Functions.ReadByte ();
			ClockPtr.month_index = Functions.ReadByte ();
			ClockPtr.year_index = Functions.ReadUShort ();
			ClockPtr.tick_count = Functions.ReadShort ();
			ClockPtr.day_in_ticks = Functions.ReadShort ();

			Vars.LastOffset += 34;
		}

		public static bool LoadGameState ()
		{
			Vars.LastOffset++;

			GSPtr.glob_flags = Functions.ReadByte ();
			GSPtr.CrewCost = Functions.ReadByte ();
			GSPtr.FuelCost = Functions.ReadByte ();

			if (GSPtr.FuelCost != 20)
				return false;

			for (int i = 0; i < 20; i++)
				GSPtr.ModuleCost[i] = Functions.ReadByte ();

			for (int i = 0; i < 8; i++)
				GSPtr.ElementWorth[i] = Functions.ReadByte ();

			GSPtr.CurrentActivity = Functions.ReadUShort ();

			LoadClockState ();

			GSPtr.autopilot_x = Functions.ReadShort ();
			GSPtr.autopilot_y = Functions.ReadShort ();
			GSPtr.ip_location_x = Functions.ReadShort ();
			GSPtr.ip_location_y = Functions.ReadShort ();
			GSPtr.ShipStamp_x = Functions.ReadShort ();
			GSPtr.ShipStamp_y = Functions.ReadShort ();
			GSPtr.ShipFacing = Functions.ReadUShort ();
			GSPtr.ip_planet = Functions.ReadByte ();
			GSPtr.in_orbit = Functions.ReadByte ();

			// VELOCITY_DESC velocity
			GSPtr.TravelAngle = Functions.ReadUShort ();
			GSPtr.vector_width = Functions.ReadShort ();
			GSPtr.vector_height = Functions.ReadShort ();
			GSPtr.fract_width = Functions.ReadShort ();
			GSPtr.fract_height = Functions.ReadShort ();
			GSPtr.error_width = Functions.ReadShort ();
			GSPtr.error_height = Functions.ReadShort ();
			GSPtr.incr_width = Functions.ReadShort ();
			GSPtr.incr_height = Functions.ReadShort ();

			return true;
		}

		public static bool LoadSisState ()
		{
			Vars.SisNameSize = 16;

			SSPtr.log_x = Functions.ReadInt ();
			SSPtr.log_y = Functions.ReadInt ();
			SSPtr.ResUnits = Functions.ReadUInt ();
			SSPtr.FuelOnBoard = Functions.ReadUInt ();
			SSPtr.CrewEnlisted = Functions.ReadUShort ();
			SSPtr.TotalElementMass = Functions.ReadUShort ();
			SSPtr.TotalBioMass = Functions.ReadUShort ();

			for (int i = 0; i < 16; i++)
				SSPtr.ModuleSlots[i] = Functions.ReadByte ();
			for (int i = 0; i < 11; i++)
				SSPtr.DriveSlots[i] = Functions.ReadByte ();
			for (int i = 0; i < 8; i++)
				SSPtr.JetSlots[i] = Functions.ReadByte ();

			SSPtr.NumLanders = Functions.ReadByte ();

			for (int i = 0; i < 8; i++)
				SSPtr.ElementAmounts[i] = Functions.ReadUShort ();

			SSPtr.ShipName = Functions.ReadStr (Vars.SisNameSize);
			SSPtr.CommanderName = Functions.ReadStr (Vars.SisNameSize);
			SSPtr.PlanetName = Functions.ReadStr (Vars.SisNameSize);

			Vars.LastOffset += 2;

			return true;
		}

		public static bool LoadSummary ()
		{
			if (Vars.SaveVersion == 1)
				SummPtr.SaveName = Functions.ReadStr (32);

			if (!LoadSisState ())
				return false;

			SummPtr.Activity = Functions.ReadByte ();
			SummPtr.Flags = Functions.ReadByte ();
			SummPtr.day_index = Functions.ReadByte ();
			SummPtr.month_index = Functions.ReadByte ();
			SummPtr.year_index = Functions.ReadUShort ();
			//SummPtr.MCreditHi = Functions.ReadByte ();
			//SummPtr.MCreditLo = Functions.ReadByte ();
			SummPtr.MCredit = Functions.ReadUShort ();
			SummPtr.NumShips = Functions.ReadByte ();
			SummPtr.NumDevices = Functions.ReadByte ();

			for (int i = 0; i < 12; i++)
				SummPtr.ShipList[i] = Functions.ReadByte ();
			for (int i = 0; i < 16; i++)
				SummPtr.DeviceList[i] = Functions.ReadByte ();

			if (Vars.SaveVersion == 1)
			{
				SummPtr.res_factor = Functions.ReadByte ();
				Vars.LastOffset++;
			}

			Vars.LastOffset++;

			return true;
		}
		public static bool LoadGame ()
		{
			if (!LoadSummary ())
				return false;

			return true;
		}
	}
}

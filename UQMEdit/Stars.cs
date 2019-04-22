using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace UQMEdit
{
	public class Stars
	{
		public static List<StarCoord> StarList = new List<StarCoord>(510);
		public static bool Filled = false;
		public static string NearestStar(double x, double y) {
			double num = 2147483647.0;
			string text = "";
			foreach (StarCoord current in Stars.StarList) {
				double num2 = Math.Sqrt((x - current.X) * (x - current.X) + (y - current.Y) * (y - current.Y));
				if (num2 < num) {
					num = num2;
					text = current.Namn;
				}
			}
			if (text.IndexOf('-') != -1) {
				text = text.Replace("-", "").Substring(1);
			}
			return text;
		}
	}
	public struct StarCoord
	{
		public double X;
		public double Y;
		public string Namn;
		public StarCoord(double x, double y, string namn) {
			this.X = x;
			this.Y = y;
			this.Namn = namn;
		}
	}
	public static class ParseStars
	{
		public static object[] LoadStars(bool spoilers) {
			List<object> list = new List<object>();
			var StarsTXT = Assembly.GetExecutingAssembly().GetManifestResourceStream("UQMEdit.Resources.stars.txt");
			StreamReader streamReader = new StreamReader(StarsTXT, Encoding.Default);
			string text = streamReader.ReadLine();
			text = streamReader.ReadLine();
			string[] array = text.Split(new char[]
			{
				'\t'
			});
			list.Add(string.Concat(new string[]
			{
				array[0],
				"\t",
				array[1],
				"\t",
				array[2],
				"\t",
				array[3].PadRight(20),
				"\t",
				array[4],
				"\t",
				array[5]
			}));
			while ((text = streamReader.ReadLine()) != null) {
				array = text.Split(new char[]
				{
					'\t'
				});
				if (array.Length >= 6 && array[4] != "-" && array[4].Split(new char[]
				{
					':'
				}).Length == 2) {
					string text2 = string.Concat(new string[]
					{
						array[0].PadRight(20),
						"\t",
						array[1],
						"\t",
						array[2],
						"\t",
						array[3].PadRight(20),
						"\t[ ",
						array[4].Split(new char[]
						{
							':'
						})[0],
						" : ",
						array[4].Split(new char[]
						{
							':'
						})[1],
						" ]"
					});
					if (spoilers) {
						text2 = text2 + "\t" + array[5];
					}
					list.Add(text2);
					if (!Stars.Filled) {
						Stars.StarList.Add(new StarCoord(double.Parse(array[4].Split(new char[]
						{
							':'
						})[0]), double.Parse(array[4].Split(new char[]
						{
							':'
						})[1]), array[1] + " " + array[0]));
					}
				}
			}
			Stars.Filled = true;
			streamReader.Close();
			return list.ToArray();
		}
	}
}

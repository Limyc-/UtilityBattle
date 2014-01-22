using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityCombat.Core;

namespace UtilityCombat
{
	class Program
	{
		static void Main(string[] args)
		{
			Game.CreateGame();

			if (!Game.Instance.Init())
			{
				Console.WriteLine("Failed to initiate game!");
				Exit(-1);
			}

			Game.Instance.MainLoop();

			Exit(0);
		}

		static void Exit(int exitCode)
		{
			Console.WriteLine("\n\nPress any key to exit...");
			Console.ReadKey();
			Environment.Exit(exitCode);
		}
	}
}

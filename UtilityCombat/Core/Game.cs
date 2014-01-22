using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityCombat.Actors;

namespace UtilityCombat.Core
{
	class Game
	{
		public static Game Instance { get; private set; }

		private bool isQuitting;
		private Actor player;
		private Actor enemy;

		public static void CreateGame()
		{
			if (Instance == null)
			{
				Instance = new Game();
			}
		}

		public Game()
		{
			player = null;
			enemy = null;
			isQuitting = false;
		}

		public bool Init()
		{
			player = new Player();
			enemy = new Enemy();

			return true;
		}

		public void MainLoop()
		{
			HandleGameTransition();

			while (!isQuitting)
			{
				ShowStatus();

				var isGameOver = player.GetNextAction(enemy);
				if (isGameOver)
				{
					HandleGameTransition();
					continue;
				}

				isGameOver = enemy.GetNextAction(player);
				if (isGameOver)
				{
					HandleGameTransition();
					continue;
				}
			}

			Instance = null;
		}

		public void ShowStatus()
		{
			Util.PrintLineSeparator();
			player.PrintStatus();
			Util.PrintLineSeparator();
			enemy.PrintStatus();
			Util.PrintLineSeparator();
			Console.WriteLine();
		}

		public void Quit() { isQuitting = true; }

		private void ResetActors()
		{
			player.Reset();
			enemy.Reset();
		}

		private void HandleGameTransition()
		{
			if (!isQuitting)
			{
				ResetActors();
				Console.Write("Your opponent, ");
				enemy.PrintName(", approaches! What will you do?\n\n");
			}
		}

	}
}

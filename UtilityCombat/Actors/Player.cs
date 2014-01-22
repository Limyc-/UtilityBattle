using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityCombat.Core;

namespace UtilityCombat.Actors
{
	class Player : Actor
	{
		public Player() : this("Player", 100, 3, 15, 20, 45, 75) { }
		public Player(string name, int maxHealth, int maxPotions, int minDamage, int maxDamage, int minHeal, int maxHeal)
			: base(name, maxHealth, maxPotions, minDamage, maxDamage, minHeal, maxHeal)
		{

		}

		protected override Core.ActionType ChooseAction(Actor target)
		{
			PrintCommands();

			while (true)
			{
				Console.Write("> ");
				var choice = Console.ReadKey().KeyChar.ToString();
				Console.WriteLine("\n");

				switch (choice)
				{
					case "1": return ActionType.Attack;
					case "2": return ActionType.Heal;
					case "3": return ActionType.Run;
					case "4":
						Game.Instance.ShowStatus();
						break;
					case "C":
					case "c":
						PrintCommands();
						break;
					case "Q":
					case "q":
						Game.Instance.Quit();
						return ActionType.None;
					default:
						PrintError("Unknown command '{0}'\n", choice);
						break;
				}
			}
		}

		private void PrintCommands()
		{
			string[] commands = { "Attack", "Heal", "Run", "Status" };
			for (int i = 0; i < commands.Length; i++)
			{
				Console.Write("{0})", i + 1);
				Console.WriteLine(" {0}", commands[i]);
			}

			Console.WriteLine("Q) Quit\n");
		}
	}
}

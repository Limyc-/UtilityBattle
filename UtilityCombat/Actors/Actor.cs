using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityCombat.Core;

namespace UtilityCombat.Actors
{
	abstract class Actor
	{
		private readonly string name;
		public string Name { get { return name; } }

		public int MaxHealth { get; private set; }
		public int CurrentHealth { get; private set; }

		public int MaxPotions { get; private set; }
		public int CurrentPotions { get; private set; }

		public int MinDamage { get; private set; }
		public int MaxDamge { get; private set; }

		public int MinHeal { get; private set; }
		public int MaxHeal { get; private set; }

		private Random rand = new Random();

		public Actor(string name, int maxHealth, int maxPotions, int minDamage, int maxDamage, int minHeal, int maxHeal)
		{
			this.name = name;
			MaxHealth = maxHealth;
			MaxPotions = maxPotions;
			MinDamage = minDamage;
			MaxDamge = maxDamage;
			MinHeal = minHeal;
			MaxHeal = maxHeal;

			CurrentHealth = MaxHealth;
			CurrentPotions = MaxPotions;
		}

		public bool GetNextAction(Actor target)
		{
			var action = ChooseAction(target);
			var result = DoAction(action, target);
			return result;
		}

		public virtual void Reset()
		{
			CurrentHealth = MaxHealth;
			CurrentPotions = MaxPotions;
		}

		public virtual void PrintStatus()
		{
			PrintName(": ".PadRight(22 - name.Length));
			Console.Write("Health: ");
			var healthRatio = (float)CurrentHealth / (float)MaxHealth;

			if (healthRatio > 0.667)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else if (healthRatio > 0.333)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}

			var healthStr = CurrentHealth.ToString();
			Console.Write(healthStr.PadRight(4));
			Console.ResetColor();
			Console.Write("| Potions: ");

			if (CurrentPotions > 1)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else if (CurrentPotions == 1)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}

			Console.WriteLine(CurrentPotions);
			Console.ResetColor();

		}

		public void PrintName(string msg = "")
		{
			if (this is Player)
			{
				Console.ForegroundColor = ConsoleColor.DarkCyan;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkMagenta;
			}

			Console.Write(name);
			Console.ResetColor();
			if (!string.IsNullOrEmpty(msg))
			{
				Console.Write(msg);
			}
		}

		public bool Hit(int damage)
		{
			CurrentHealth -= damage;
			if (CurrentHealth <= 0)
			{
				PrintName(" is dead!\n\n");
				return true;
			}
			return false;
		}

		protected abstract ActionType ChooseAction(Actor target);

		protected virtual void PrintError(string msg, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(msg, args);
			Console.ResetColor();
		}

		private bool DoAction(ActionType action, Actor target)
		{
			switch (action)
			{
				case ActionType.Attack: return Attack(target);
				case ActionType.Heal:
					Heal();
					return false;
				case ActionType.Run: return Run();
				case ActionType.None: return true;
				default:
					PrintError("[ERROR] Unknown action '{0}'\n", action);
					break;
			}
			return false;
		}

		private bool Attack(Actor target)
		{
			var damage = (rand.Next() % (MaxDamge - MinDamage)) + MinDamage;
			PrintName(" struck ");
			target.PrintName(" for ");
			PrintDamage(damage, " damage\n\n");

			return target.Hit(damage);
		}

		private void Heal()
		{
			if (CurrentPotions > 0)
			{
				var healAmount = (rand.Next() % (MaxHeal - MinHeal)) + MinHeal;
				PrintName(" healed for ");
				PrintDamage(healAmount, " HP\n\n", true);

				CurrentHealth = (CurrentHealth + healAmount).Clamp(0, MaxHealth);
				CurrentPotions--;
			}
			else
			{
				PrintName(" could not heal due to lack of potions\n\n");
			}
		}

		private bool Run()
		{
			if (rand.NextDouble() < 0.5)
			{
				PrintName(" ran away!\n\n");
				return true;
			}
			else
			{
				PrintName(" failed to run away!\n\n");
				return false;
			}
		}

		private void PrintDamage(int damage, string msg = "", bool isHealing = false)
		{
			if (isHealing)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}

			Console.Write(damage);
			Console.ResetColor();
			if (!string.IsNullOrEmpty(msg))
			{
				Console.Write(msg);
			}
		}

	}
}

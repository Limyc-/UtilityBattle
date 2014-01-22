using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityCombat.Core;

namespace UtilityCombat.Actors
{
	class Enemy : Actor
	{
		private readonly float BaseAttackScore = 0.4f;
		private readonly float RunScoreExponentCoefficient = 0.25f;
		private readonly float RunScorePotionEffectExponent = 3f;
		private readonly float HealScoreLogisticRange = 6f;
		private readonly float HealScoreSteepness = 1.848431643f; // e * 0.68

		private Dictionary<ActionType, float> lastScores = new Dictionary<ActionType, float>();
		private Random rand = new Random();

		public Enemy() : this("Utility Curve", 100, 2, 10, 25, 45, 75) { }
		public Enemy(string name, int maxHealth, int maxPotions, int minDamage, int maxDamage, int minHeal, int maxHeal)
			: base(name, maxHealth, maxPotions, minDamage, maxDamage, minHeal, maxHeal)
		{

		}

		public override void Reset()
		{
			base.Reset();
			lastScores.Clear();
		}

		protected override ActionType ChooseAction(Actor target)
		{
			float total = 0;
			lastScores.Clear();

			// threat score
			float threat = Math.Min(target.MaxDamge / (float)CurrentHealth, 1f);

			// attack
			float score = ScoreAttack(target.CurrentHealth);
			total += score;
			lastScores[ActionType.Attack] = score;

			// heal
			score = ScoreHeal(threat);
			total += score;
			lastScores[ActionType.Heal] = score;

			// run
			score = ScoreRun(threat);
			total += score;
			lastScores[ActionType.Run] = score;

			// sort from highest to lowest score
			lastScores = lastScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

			PrintUtilityScores();

			// weighted random
			float choice = (float)rand.NextDouble() * total;
			float accumulation = 0f;
			foreach (var pair in lastScores)
			{
				accumulation += pair.Value;
				if (choice < accumulation)
				{
					return pair.Key;
				}
			}

			PrintError("[ERROR] Failed to find action. Check weighted random code for bugs");
			return ActionType.Run;
		}

		private float ScoreAttack(int targetHP)
		{
			float inverseRatio = 1 - ((float)(targetHP - MinDamage) / (MaxDamge - MinDamage));
			float score = (inverseRatio * (1 - BaseAttackScore)) + BaseAttackScore;
			return Math.Max(Math.Min(score, 1f), BaseAttackScore);
		}

		private float ScoreRun(float threatScore)
		{
			float ratio = ((float)CurrentHealth / MaxHealth);
			float expoent = (float)(1 / Math.Pow(((float)CurrentPotions + 1f), RunScorePotionEffectExponent)) * RunScoreExponentCoefficient;
			float score = 1 - (float)Math.Pow(ratio, expoent);
			return score * threatScore;
		}

		private float ScoreHeal(float threatScore)
		{
			if (CurrentPotions == 0) return 0;

			float exponent = -(((float)CurrentHealth / MaxHealth) * (HealScoreLogisticRange * 2)) + HealScoreLogisticRange;
			float denominator = 1 + (float)Math.Pow(HealScoreSteepness, exponent);
			float score = 1 - (1 / denominator);
			return score * threatScore;
		}

		private void PrintUtilityScores()
		{
			Util.PrintLineSeparator();
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			var title = "AI Utility Scores";
			Console.WriteLine(title.PadLeft(25 + (title.Length / 2)));
			Console.ResetColor();
			Util.PrintLineSeparator();

			foreach (var pair in lastScores)
			{
				Console.Write("\t    ");
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				string key = pair.Key.ToString() + ": ";
				Console.Write(key);
				Console.ResetColor();
				//Console.Write(": ");
				string val = " " + pair.Value.ToString("0.00########");
				Console.WriteLine(val.PadLeft(12 + val.Length + 8 - key.Length, '.'));
				Console.ResetColor();
			}

			Util.PrintLineSeparator();
			Console.WriteLine();
		}
	}
}

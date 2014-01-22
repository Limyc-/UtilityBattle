using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCombat.Core
{
	class Util
	{
		public static void PrintLineSeparator(int length = 50)
		{
			string separator = "";

			for (int i = 0; i < length; i++)
			{
				separator += "-";
			}

			Console.WriteLine(separator);
		}
	}
}

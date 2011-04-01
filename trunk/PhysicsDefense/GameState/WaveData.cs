using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsDefense.GameState
{
	/// <summary>
	/// Contains balance numbers related to enemy wave interaction.
	/// Should also contain data on what enemies are in each wave in the future.
	/// </summary>
	public class WaveData
	{
		public static float baseBounty = 30f;
		public static float waveBountyMult = 5f;

		public static int initialEnemyCount = 10;
		public static int extraEnemiesPerWave = 1;

		public static float healthMult = 0.45f;
		public static float bountyMult = 0.2f;
	}
}

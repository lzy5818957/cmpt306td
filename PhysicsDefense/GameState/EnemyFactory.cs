using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using PhysicsDefense.Physics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
	/// <summary>
	/// Creates enemies.
	/// Will need to be redone so it's more like a factory.
	/// </summary>
	class EnemyFactory
	{
		public static Marble createMarble(Vector2 position, PhysicsSystem physics)
		{
			Marble m = new Marble(physics.world, position);
			return m;
		}

		public static Box createBox(Vector2 position, PhysicsSystem physics)
		{
			Box b = new Box(physics.world, position);
			return b;
		}
	}
}

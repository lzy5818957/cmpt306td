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
			Marble m = new Marble();
			m.physicsProperties.fixture = FixtureFactory.CreateCircle(physics.world, 25.0f, 3.0f, position);
			m.physicsProperties.fixture.Body.BodyType = BodyType.Dynamic;
			return m;
		}

		public static Box createBox(Vector2 position, PhysicsSystem physics)
		{
			Box b = new Box();
			b.physicsProperties.fixture = FixtureFactory.CreateRectangle(physics.world, 50f, 50f, 6f, position);
			b.physicsProperties.fixture.Body.BodyType = BodyType.Static;
			return b;
		}
	}
}

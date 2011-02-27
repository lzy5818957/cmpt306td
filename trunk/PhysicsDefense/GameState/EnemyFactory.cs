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
	class EnemyFactory
	{
		public static Marble createMarble(Vector2 position, PhysicsSystem physics)
		{
			Marble m = new Marble();
			m.physicsProperties.fixture = FixtureFactory.CreateCircle(physics.world, 30.0f, 3.0f, position);
			m.physicsProperties.fixture.Body.BodyType = BodyType.Dynamic;
			return m;
		}
	}
}

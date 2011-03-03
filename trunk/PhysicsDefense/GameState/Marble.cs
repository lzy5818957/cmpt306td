using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
	class Marble : GameObject
	{
		public Marble(World world, Vector2 position)
		{
			spriteName = "puck";
			physicsProperties.fixture = FixtureFactory.CreateCircle(world, 25.0f, 3.0f, position);
			physicsProperties.fixture.Body.BodyType = BodyType.Dynamic;
		}
	}
}

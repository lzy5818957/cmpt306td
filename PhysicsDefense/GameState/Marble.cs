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
			physicsProperties.fixture = FixtureFactory.CreateCircle(world, 0.25f, 3.0f, position);
			physicsProperties.fixture.Restitution = 0.2f;
			physicsProperties.fixture.Body.BodyType = BodyType.Dynamic;
			physicsProperties.fixture.Friction = 0.8f;
		}

        public override void update()
        {

        }

	}
}

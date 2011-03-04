using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
	class Box : GameObject
	{
		private static float width = 6f;
		private static float height = 6f;
		private static float density = 6f;

		public Box(World world, Vector2 position)
		{
			spriteName = "box";
			physicsProperties.fixture = FixtureFactory.CreateRectangle(world, width, height, density, position);
			physicsProperties.fixture.Restitution = 0.2f;
			physicsProperties.fixture.Body.BodyType = BodyType.Static;
			physicsProperties.fixture.Friction = 0.8f;
		}

        public override void update()
        {

        }
	}
}

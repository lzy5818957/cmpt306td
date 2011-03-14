using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDefense.GameState
{
	class Tower : GameObject
	{
		private static float radius = 0.25f;
		private static float density = 5.0f;

		public Tower(World world, Vector2 position)
		{
			spriteName = "puck";
			physicsProperties.fixture = FixtureFactory.CreateCircle(world, radius, density);
			physicsProperties.fixture.Restitution = 0.2f;
			physicsProperties.fixture.Friction = 0.8f;

			physicsProperties.fixture.Body.BodyType = BodyType.Dynamic;
			physicsProperties.fixture.IsSensor = true;
			physicsProperties.fixture.Body.IgnoreGravity = true;

			nativeColor = Color.White;
			color.A = 128;
		}

		public override void activate()
		{
			physicsProperties.fixture.IsSensor = false;
			physicsProperties.fixture.Body.IgnoreGravity = false;
			physicsProperties.fixture.Body.BodyType = BodyType.Static;
			base.activate();
		}

		public override void update()
		{
		}
	}
}

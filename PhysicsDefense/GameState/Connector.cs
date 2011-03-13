using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace PhysicsDefense.GameState
{
	class Connector : GameObject
	{
		public float height;
		public float endRadius;

		public Connector(World world, float height, float endRadius)
		{
			spriteName = "connector";
			this.height = height;
			this.endRadius = endRadius;

			physicsProperties.fixtureList = FixtureFactory.CreateCapsule(world, height, endRadius, 1f);
			physicsProperties.fixture[0].Restitution = 0.2f;
			physicsProperties.fixture[0].Body.BodyType = BodyType.Static;
			physicsProperties.fixture[0].Friction = 0.8f;
		}

		public override void update()
		{
		}
	}
}

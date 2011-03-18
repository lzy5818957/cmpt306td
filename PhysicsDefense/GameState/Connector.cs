using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
	class Connector : GameObject
	{
		public float height;
		public float width;

		public Connector(World world, float width, float height)
		{
			spriteName = "connector";
			this.height = height;
			this.width = width;

			//physicsProperties.fixtureList = FixtureFactory.CreateCapsule(world, height, endRadius, 1f);
			physicsProperties.body = BodyFactory.CreateRectangle(world, width, height, 1f);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Static;
			physicsProperties.body.Friction = 0.8f;
			physicsProperties.body.CollisionCategories = Category.Cat3;
			physicsProperties.body.CollidesWith = Category.Cat1;
		}

		public override void update(GameTime gameTime)
		{
			base.update(gameTime);
		}
	}
}

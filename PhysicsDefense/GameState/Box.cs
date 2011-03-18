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
		private static float width = 0.6f;
		private static float height = 0.6f;
		private static float density = 0.6f;

		public Box(World world, Vector2 position)
		{
			spriteName = "box";
			physicsProperties.body = BodyFactory.CreateRectangle(world, width, height, density, position);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Static;
			physicsProperties.body.Friction = 0.8f;
		}

        public override void update(GameTime gameTime)
        {
			base.update(gameTime);
        }
	}
}

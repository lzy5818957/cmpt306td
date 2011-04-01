using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
	public class Spinner : GameObject
	{
		float radius;

		public Spinner(World world, Vector2 position, float radius)
		{
			physicsProperties.body = BodyFactory.CreateCircle(world, radius, 3.0f, position);
			physicsProperties.body.AngularDamping = 1f;
			physicsProperties.body.CollisionCategories = Category.None;
			physicsProperties.body.CollidesWith = Category.None;
			physicsProperties.body.BodyType = BodyType.Dynamic;
			physicsProperties.body.IgnoreGravity = true;

			this.position = position;
			this.radius = radius;

			this.spriteName = "spinner";
		}
	}
}

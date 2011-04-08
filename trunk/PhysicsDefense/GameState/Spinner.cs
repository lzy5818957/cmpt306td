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
		private float maxSpinVelocity = 50f;

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

		public override void update(GameTime gameTime)
		{
			if (physicsProperties.body.AngularVelocity > maxSpinVelocity)
				physicsProperties.body.AngularVelocity = maxSpinVelocity;
			if (physicsProperties.body.AngularVelocity < maxSpinVelocity * -1f)
				physicsProperties.body.AngularVelocity = maxSpinVelocity * -1f;
			color.A = (byte)(255f * Math.Abs(physicsProperties.body.AngularVelocity / maxSpinVelocity));
            color.B = (byte)(255f * Math.Abs(physicsProperties.body.AngularVelocity / maxSpinVelocity));
            color.G = (byte)(255f * Math.Abs(physicsProperties.body.AngularVelocity / maxSpinVelocity));
            color.R = (byte)(255f * Math.Abs(physicsProperties.body.AngularVelocity / maxSpinVelocity));
			base.update(gameTime);
		}
	}
}

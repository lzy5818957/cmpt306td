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
		public int collisionCount = 0;
		private World world;

		float range = 1f;
		public AoeSensor rangeSensor;

		public Tower(World world, Vector2 position)
		{
			this.world = world;
			spriteName = "puck";
			physicsProperties.body = BodyFactory.CreateCircle(world, radius, density);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.Friction = 0.8f;

			physicsProperties.body.BodyType = BodyType.Dynamic;
			physicsProperties.body.IsSensor = true;
			physicsProperties.body.IgnoreGravity = true;
			physicsProperties.body.CollisionCategories = Category.Cat2;

			physicsProperties.body.OnCollision += (a, b, c) => { collisionCount++;  return true; };
			physicsProperties.body.OnSeparation += (a, b) => { collisionCount--; };

			nativeColor = Color.White;
			color.A = 128;
		}

		public void activate()
		{
			color = nativeColor;
			color.A = 255;

			physicsProperties.body.IsSensor = false;
			physicsProperties.body.IgnoreGravity = true;
			//physicsProperties.body.CollidesWith = Category.None;
			physicsProperties.body.BodyType = BodyType.Static;

			rangeSensor = new AoeSensor(world, position, range);
		}

		public override void update()
		{
			base.update();
		}
	}
}

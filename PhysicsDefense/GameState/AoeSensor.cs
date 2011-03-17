using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace PhysicsDefense.GameState
{
	class AoeSensor : GameObject
	{
		private float radius;
		private Body body;

		public AoeSensor(World world, Vector2 position, float radius)
		{
			//this.position = position;
			this.radius = radius;
			this.spriteName = null;

			body = BodyFactory.CreateCircle(world, radius, 1f, position);
			body.BodyType = BodyType.Static;
			body.IsSensor = true;
			body.CollisionCategories = Category.Cat4;  //sensor category
			body.CollidesWith = Category.Cat1;  //only collides with marbles

			body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
			body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
		}

		bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			Console.WriteLine("Tower range sensor triggered");
			return false;
		}

		void body_OnSeparation(Fixture fixtureA, Fixture fixtureB) {
			Console.WriteLine("Tower range sensor un-triggered");
		}
	}
}

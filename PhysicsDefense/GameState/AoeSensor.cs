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
		private Vector2 sensorPos;
		private float radius;
		private Body body;

		public AoeSensor(World world, Vector2 position, float radius)
		{
			this.position = position;
			this.radius = radius;
			this.spriteName = null;

			body = BodyFactory.CreateCircle(world, radius, 1f);
			body.BodyType = BodyType.Dynamic;
			body.IsSensor = true;

			body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
		}

		bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			return false;
		}
	}
}

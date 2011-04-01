using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;

namespace PhysicsDefense.GameState
{
    public delegate void RangeHandler(Marble marble);

	public class AoeSensor : GameObject
	{
		private float radius;
		public Body body;

        public RangeHandler onEnter;
        public RangeHandler onLeave;

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
            Debug.Assert(fixtureB.Body.UserData != null);

            onEnter((Marble)fixtureB.Body.UserData);
            return true;
		}

		void body_OnSeparation(Fixture fixtureA, Fixture fixtureB) {
			Debug.Assert(fixtureB.Body.UserData != null);
			onLeave((Marble)fixtureB.Body.UserData);
		}
	}
}

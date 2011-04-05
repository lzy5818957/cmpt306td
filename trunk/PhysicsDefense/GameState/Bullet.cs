using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    public class Bullet : GameObject
    {
        private static float density = 0.01f;
        private static float radius = 0.05f;
        private double age = 0;

		private int damage = 15;
		public static float speed = 6f;

        public Bullet(World world, Vector2 position, Vector2 direction) {
			this.world = world;
            spriteName = "basicbullet";
			physicsProperties.body = BodyFactory.CreateCircle(world, radius, density, position);
            physicsProperties.body.BodyType = BodyType.Dynamic;
			physicsProperties.body.IsSensor = true;
            physicsProperties.body.CollisionCategories = Category.Cat5;
            physicsProperties.body.CollidesWith = Category.Cat1;
            physicsProperties.body.IgnoreGravity = true;

			direction.Normalize();
			physicsProperties.body.LinearVelocity = direction * speed;

			physicsProperties.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        public override void initialize()
        {
            onPlaySound("cannonshot");
            base.initialize();
        }

		bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
            if(typeof(Marble) == fixtureB.Body.UserData.GetType())
            {
			    Marble m = (Marble)fixtureB.Body.UserData;
			    m.takeDamage(damage);
                this.die();
                return true;
            }
            else
            {
                this.die();
                return false;
            }
		}

		public override void update(GameTime gameTime)
        {
			base.update(gameTime);

			age += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (age >= 1000)
				die();

        }

        public override void die()
        {
            Smoke smoke = new Smoke(world, position, "bullethit", 0.12f);
            onCreateObject(smoke);
            base.die();
        }

    }
}

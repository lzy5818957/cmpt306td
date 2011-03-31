using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    class Missile:GameObject
    {
        private static float density = 0.01f;
        private static float radius = 0.2f;
        private double age = 0;

		private int damage = 15;
		private float strength = 15f;
        private GameObject target;

        public Missile(World world, Vector2 position, GameObject tgt)
        {
            this.world = world;
            target = tgt;
            spriteName = "missile";
            physicsProperties.body = BodyFactory.CreateCircle(world, radius, density, position);
            physicsProperties.body.BodyType = BodyType.Dynamic;
            physicsProperties.body.IsSensor = true;
            physicsProperties.body.CollisionCategories = Category.Cat5;
            physicsProperties.body.CollidesWith = Category.Cat1;
            physicsProperties.body.IgnoreGravity = true;
            physicsProperties.body.Mass = 1f;
            Vector2 direction = new Vector2(target.position.X - this.position.X, target.position.Y - this.position.Y);
            direction.Normalize();
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            physicsProperties.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            
        }

        public override void initialize()
        {
            onPlaySound("missilelaunch");
            base.initialize();
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Marble m = (Marble)fixtureB.Body.UserData;
            m.takeDamage(damage);
            this.die();
            return true;
        }

        public override void die()
        {
            Explode explosion = new Explode(world, position, 0.36f);
            onCreateObject(explosion);
            base.die();
        }

        public override void update(GameTime gameTime)
        {
            if (target != null&&target.isDead==false)
            {
                Vector2 direction = new Vector2(target.position.X - this.position.X, target.position.Y - this.position.Y);
                direction.Normalize();
                physicsProperties.body.ApplyForce(direction * strength);
                rotation = (float)Math.Atan2(direction.Y,direction.X);
            }
            if (target.isDead)
                die();

            base.update(gameTime);

            age += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (age >= 2000)
                die();

        }
    }
}

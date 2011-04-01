using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    class AoeMissile : GameObject
    {
        private static float density = 0.01f;
        private static float radius = 0.2f;
        private double age = 0;
        public float range=1f;
        private int damage = 20;
        private float strength = 5f;
        private GameObject target;

        public AoeMissile(World world, Vector2 position, GameObject tgt)
        {
            this.world = world;
            target = tgt;
            spriteName = "aoemissile";
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
            if (typeof(Marble) == fixtureB.Body.UserData.GetType())
            {
                Marble m = (Marble)fixtureB.Body.UserData;
                //m.takeDamage(damage);
                for (int i = 0; i < Marble.marbles.Count; i++)
                {
                    Marble tgt = Marble.marbles[i];
                    if (tgt == null)
                        continue;
                    Vector2 distance = new Vector2((position.X - tgt.position.X), (position.Y - tgt.position.Y));
                    if (distance.Length() < range)
                        tgt.takeDamage(damage);
                }
                Smoke smoke = new Smoke(world, position, "aoesmoke", range);
                onCreateObject(smoke);
                this.die();
                return true;
            }
            else
            {
                this.die();
                return false;
            }
            
        }

        public override void die()
        {
            onPlaySound("missilehit");
            base.die();
        }

        public override void update(GameTime gameTime)
        {
            if (target != null && target.isDead == false)
            {
                Vector2 direction = new Vector2(target.position.X - this.position.X, target.position.Y - this.position.Y);
                direction.Normalize();
                physicsProperties.body.LinearVelocity = direction * strength;
                rotation = (float)Math.Atan2(direction.Y, direction.X);
                //physicsProperties.body.ApplyForce(direction * strength);
                //Vector2 speed = physicsProperties.body.LinearVelocity;
                //speed.Normalize();
                //physicsProperties.body.LinearVelocity = speed * strength;
                
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

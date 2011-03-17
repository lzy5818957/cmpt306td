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
    public delegate void BulletHandler(Bullet bullet);

	class Tower : GameObject
	{
		private static float radius = 0.25f;
		private static float density = 5.0f;
		public int collisionCount = 0;
		private World world;
        public int rechargeTime=30;
        private int rechargeCount=0;
		public float range = 1f;
		public AoeSensor rangeSensor;
        public bool isActivated = false;

        List<Marble> enemiesInRange;
        public BulletHandler onBulletCreate;

		public Tower(World world, Vector2 position)
		{
            enemiesInRange = new List<Marble>();
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
            isActivated = true;
			physicsProperties.body.IsSensor = false;
			physicsProperties.body.IgnoreGravity = true;
			//physicsProperties.body.CollidesWith = Category.None;
			physicsProperties.body.BodyType = BodyType.Static;

			rangeSensor = new AoeSensor(world, position, range);
            rangeSensor.onEnter = enemyEnter;
            rangeSensor.onLeave = enemyLeave;
		}

        public void enemyEnter(Marble m)
        {
            enemiesInRange.Add(m);
        }

        public void enemyLeave(Marble m)
        {
            enemiesInRange.Remove(m);
        }

        public Bullet shoot()
        {
            if (enemiesInRange.Count == 0)
                return null;

            Marble target = enemiesInRange[0];
            if (rechargeCount >= rechargeTime)
            {
                rechargeCount = 0;
                Bullet newBullet = new Bullet(world, position);
                Vector2 displacement=new Vector2((target.position.X - position.X), (target.position.Y - position.Y));
                displacement.Normalize();
                //newBullet.physicsProperties.body.ApplyLinearImpulse(new Vector2(-0.001f*displacement.X/displacement.Length(),0.001f*displacement.Y/displacement.Length()));
                newBullet.physicsProperties.body.LinearVelocity = displacement * 5;
                onBulletCreate(newBullet);
                return newBullet;
            }
            return null;
        }

		public override void update()
		{
            if (!isActivated)
                return;

            rechargeCount++;
            if (rechargeCount > rechargeTime)
                rechargeCount = rechargeTime;
            shoot();
			base.update();
		}
	}
}

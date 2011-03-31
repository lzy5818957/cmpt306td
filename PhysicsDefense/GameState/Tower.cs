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

		// Game-related properties
        private double rechargeTime=500;
		public float range = 2f;
		public static float cost = 30f;

		public AoeSensor rangeSensor;
        public bool isActivated = false;

		private double timer = 0;

        List<Marble> enemiesInRange;

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
			physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat3;
			physicsProperties.body.CollisionCategories = Category.Cat2;

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

        public void shoot()
        {
            if (enemiesInRange.Count == 0)
                return;
			Marble target = enemiesInRange[0];
			
			//Vector2 direction = new Vector2((target.position.X - position.X), (target.position.Y - position.Y));
            //Bullet newBullet = new Bullet(world, position, direction);
            //onCreateObject(newBullet);
            Missile newMissile = new Missile(world, position, target);
			onCreateObject(newMissile);
        }

		public override void update(GameTime gameTime)
		{
			base.update(gameTime);

            if (!isActivated)
                return;

			enemiesInRange.RemoveAll(delegate(Marble obj) { return obj.isDead; });

			timer += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timer >= rechargeTime) {
				timer -= rechargeTime;
				shoot();
			}
		}
	}
}

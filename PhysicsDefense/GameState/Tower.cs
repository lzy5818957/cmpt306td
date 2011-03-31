﻿using System;
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
        public double rechargeTime;
		public float range;
		public static float cost;

		public AoeSensor rangeSensor;
        public bool isActivated = false;

		private double timer = 0;

        protected List<Marble> enemiesInRange;

		public Tower(World world, Vector2 position)
		{
            enemiesInRange = new List<Marble>();
			this.world = world;
			
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

        public virtual void shoot()
        {
        }

		public override void update(GameTime gameTime)
		{
			base.update(gameTime);

            if (!isActivated)
                return;
            if (enemiesInRange.Count != 0)
            {
                Marble target = enemiesInRange[0];
                rotation = (float)Math.Atan2(target.position.Y - position.Y, target.position.X - position.X);
            }
            else
            {
                rotation += 0.01f;
            }
			enemiesInRange.RemoveAll(delegate(Marble obj) { return obj.isDead; });

			timer += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timer >= rechargeTime) {
				timer -= rechargeTime;
				shoot();
			}
		}
	}
}

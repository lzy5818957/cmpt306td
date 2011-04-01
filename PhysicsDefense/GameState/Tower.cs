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
        private float _range;

        public double rechargeTime;
		public float range {
            get {
                return _range;
            }
            set {
                _range = value;
                if (rangeSensor == null)
                    return;

                if(world.BodyList.Contains(rangeSensor.body))
                    world.RemoveBody(rangeSensor.body);
                this.rangeSensor = new AoeSensor(world, position, value);
                rangeSensor.onEnter = enemyEnter;
                rangeSensor.onLeave = enemyLeave;
            }
        }

		public static float cost;

		public AoeSensor rangeSensor;
		float spinTransferFactor = 0.001f;
		public Spinner spinner;
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

            this.rangeSensor = new AoeSensor(world, position, _range);
            rangeSensor.onEnter = enemyEnter;
            rangeSensor.onLeave = enemyLeave;

			//spinSensor = new AoeSensor(world, position, radius * 1.2);
			spinner = new Spinner(world, position, radius * 2f);
			onCreateObject(spinner);
		}

		public void applySpin(float torque)
		{
			spinner.physicsProperties.body.ApplyTorque(torque);
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

			// Spin
			foreach (Marble target in enemiesInRange) {
				float distance = (target.position - this.position).Length();
				if (distance < radius * 2f) {
					target.physicsProperties.body.ApplyAngularImpulse(spinner.physicsProperties.body.AngularVelocity * spinTransferFactor);
				}
			}

			// Target enemy
            if (enemiesInRange.Count != 0) {
                Marble target = enemiesInRange[0];
                rotation = (float)Math.Atan2(target.position.Y - position.Y, target.position.X - position.X);
            } else {
                rotation += 0.01f;
            }
			enemiesInRange.RemoveAll(delegate(Marble obj) { return obj.isDead; });

			// Shot reload time
			timer += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timer >= rechargeTime) {
				timer -= rechargeTime;
				shoot();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhysicsDefense.GameState
{
	class Tower : GameObject
	{
        public static List<Tower> towers = new List<Tower>();
		public float radius = 0.25f;
		private static float density = 5.0f;
        private float _range;

        public double rechargeTime;

		public static float cost;
        public bool isSelected;
		public AoeSensor rangeSensor;
        public RangeIndicator rangeIndicator=null;
		float spinTransferFactor = 0.001f;
		public Spinner spinner;
        public bool isActivated = false;

		private double timer = 0;

        protected List<Marble> enemiesInRange;

		public float range
		{
			get
			{
				return _range;
			}
			set
			{
				_range = value;
				if (rangeSensor == null)
					return;

				if (world.BodyList.Contains(rangeSensor.body))
					world.RemoveBody(rangeSensor.body);
				this.rangeSensor = new AoeSensor(world, position, value);
				rangeSensor.onEnter = enemyEnter;
				rangeSensor.onLeave = enemyLeave;
			}
		}

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
			physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat3 | Category.Cat6;
			physicsProperties.body.CollisionCategories = Category.Cat2;
			nativeColor = Color.White;
			color.A = 127;
            color.B = 127;
            color.G = 127;
            color.R = 127;
		}

		public void activate()
		{
            rangeIndicator.isVisible = false;
			color = nativeColor;
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
            towers.Add(this);
		}

		public void applySpin(float torque)
		{
            if (spinner != null)
            {
                spinner.physicsProperties.body.ApplyTorque(torque);
            }
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

        public void checkSelected(MouseState state) { 
            if(
                state.LeftButton == ButtonState.Pressed
                    && state.X/GameWorld.worldScale <= position.X+0.25f
                    && state.X/GameWorld.worldScale >= position.X-0.25f
                    && state.Y/GameWorld.worldScale <= position.Y+0.25f
                    && state.Y/GameWorld.worldScale >= position.Y-0.25f
                    && isActivated
            ){
                isSelected = true;
                if(rangeIndicator!=null)
                    rangeIndicator.isVisible = true;
            }

            else{
                isSelected = false;
                if(rangeIndicator!=null)
                    rangeIndicator.isVisible= false;
            }
        }

		public override void update(GameTime gameTime)
		{
			base.update(gameTime);
            if (rangeIndicator == null)
            {
                rangeIndicator = new RangeIndicator(world, position, range); 
                onCreateObject(rangeIndicator);
            }

            if(rangeIndicator.range != range)
            {
                onDeath(rangeIndicator);
                rangeIndicator = null;
                rangeIndicator = new RangeIndicator(world, position, range);
                if (!isSelected)
                    rangeIndicator.isVisible = false;
                onCreateObject(rangeIndicator);
            }

            if (!isActivated)
            {
                foreach (Tower tower in towers)
                {
                    Vector2 distance = new Vector2(this.position.X - tower.position.X, this.position.Y - tower.position.Y);
                    if (distance.Length() <= 0.5f)
                        this.isColliding = true;
                }
                rangeIndicator.position = position;
                return;
            }
            if (!isActivated || isDead)
                return;

			// Spin
			spinner.update(gameTime);
			foreach (Marble target in enemiesInRange) {
				float distance = (target.position - this.position).Length();
				if (distance < radius * 3f) {
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

        public float sell()
        {
            List<Connector> removeList=new List<Connector>();
            foreach (Connector con in Connector.connectors)
            {
                if (con == null)
                    continue;
                if (con.towerA == this || con.towerB == this)
                {
                    if (world.BodyList.Contains(rangeSensor.body))
                        world.RemoveBody(rangeSensor.body);
                    removeList.Add(con);
                }
            }
            foreach (Connector con in removeList)
                con.die();
            die();
            return cost;
        }

        public override void die()
        {
            onDeath(rangeIndicator);
            towers.Remove(this);
            base.die();
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
	class Connector : GameObject
	{
		public float height;
		public float width;
        public Tower towerA;
        public Tower towerB;
        public static List<Connector> connectors=new List<Connector>();
		public Connector(World world, float width, float height,Tower t1,Tower t2)
		{
			spriteName = "connector";
			this.height = height;
			this.width = width;
            towerA = t1;
            towerB = t2;
			//physicsProperties.fixtureList = FixtureFactory.CreateCapsule(world, height, endRadius, 1f);
			physicsProperties.body = BodyFactory.CreateRectangle(world, width, height, 1f);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Static;
			physicsProperties.body.Friction = 0.8f;
			physicsProperties.body.CollisionCategories = Category.Cat3;
			physicsProperties.body.CollidesWith = Category.Cat1;
            connectors.Add(this);
            position = (towerA.position + towerB.position) / 2f;
            rotation = (float)Math.Atan2((towerA.position.Y - towerB.position.Y), (towerA.position.X - towerB.position.X));
            
		}

		public override void update(GameTime gameTime)
		{
			base.update(gameTime);
		}

        public override void die()
        {
            connectors.Remove(this);
            base.die();
        }
	}
}

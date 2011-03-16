using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    class Explode : GameObject
    {
        public int frameCount;
        public Explode(World world, Vector2 position)
        {
            frameCount = 1;
            spriteName = "explode1";
            physicsProperties.body = BodyFactory.CreateCircle(world, 1.44f, 1f, position);

            //No collision
            physicsProperties.body.CollidesWith = Category.None;
            physicsProperties.body.BodyType = BodyType.Static;
        }

		public override void initialize()
		{
			onPlaySound("explode");
			base.initialize();
		}

        public override void update()
        {
			if (frameCount < 17) {
				frameCount++;
				spriteName = "explode" + frameCount.ToString();
			} else {
				die();
			}
        }
    }
}

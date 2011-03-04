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
            physicsProperties.fixture = FixtureFactory.CreateCircle(world, 14.4f, 14.4f, position);
            //No collision
            physicsProperties.fixture.CollisionFilter.CollidesWith = Category.None;
            physicsProperties.fixture.Body.BodyType = BodyType.Static;
            
        }

        public override void update()
        {
            if (frameCount < 17)
            {
                frameCount++;
                spriteName = "explode" + frameCount.ToString();
            }
        }
    }
}

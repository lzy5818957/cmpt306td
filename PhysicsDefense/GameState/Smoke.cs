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
    class Smoke:GameObject
    {
        int frameCount = 0;
        public Smoke(World world, Vector2 position,String sN,float radius)
        {
            spriteName = sN;
            physicsProperties.body = BodyFactory.CreateCircle(world, radius, 1f, position);

            //No collision
            physicsProperties.body.CollidesWith = Category.None;
            physicsProperties.body.CollisionCategories = Category.None;
            physicsProperties.body.BodyType = BodyType.Static;
        }
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            //size.X += 0.01f;
            //size.Y += 0.01f;
            //color.A -= 1;
            frameCount++;
            color.A = (byte)(255 - 16*frameCount);
            if (frameCount>=16)
            {
                die();
            }
        }
    }
}

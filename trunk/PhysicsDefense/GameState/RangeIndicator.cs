using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    class RangeIndicator:GameObject
    {
        public float range;
        public static bool isVisibleAll=false;
        public bool isVisible;
        public RangeIndicator(World world, Vector2 position, float radius)
        {
            range = radius;
            spriteName = "rangeindicator";
            physicsProperties.body = BodyFactory.CreateCircle(world, range, 1f, position);
            color.A = 0;
            isVisible = true;
            //No collision
            physicsProperties.body.CollidesWith = Category.None;
            physicsProperties.body.CollisionCategories = Category.None;
            physicsProperties.body.BodyType = BodyType.Static;
        }

        public override void update(GameTime gameTime)
        {
            if (isVisibleAll)
            {
                if (isVisible)
                    color.A =127;
                else
                    color.A =63;
            }
            else
            {
                if (isVisible)
                    color.A = 63;
                else
                    color.A = 0;
            }
            base.update(gameTime);
        }
    }
}

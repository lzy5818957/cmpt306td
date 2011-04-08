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
            color=Color.Transparent;
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
                {
                    color.A = 127;
                    color.B = 127;
                    color.G = 127;
                    color.R = 127;
                }
                else
                {
                    color.A = 63;
                    color.B = 63;
                    color.G = 63;
                    color.R = 63;
                }
            }
            else
            {
                if (isVisible)
                {
                    color.A = 63;
                    color.B = 63;
                    color.G = 63;
                    color.R = 63;
                }
                else
                    color = Color.Transparent;
            }
            base.update(gameTime);
        }
    }
}

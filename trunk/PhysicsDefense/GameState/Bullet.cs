using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    public class Bullet : GameObject
    {
        private static float density = 0.01f;
        private static float radius = 0.05f;
        private int age=0;

        public Bullet(World world, Vector2 position) { 
            spriteName = "puck";
			physicsProperties.body = BodyFactory.CreateCircle(world, radius, density, position);
            physicsProperties.body.BodyType = BodyType.Dynamic;
            physicsProperties.body.CollisionCategories = Category.Cat5;
            physicsProperties.body.CollidesWith = Category.Cat1;
            physicsProperties.body.IgnoreGravity = true;

        }
        public override void update()
        {
            age++;
            if (age > 60)
                die();
            base.update();
        }
    }
}

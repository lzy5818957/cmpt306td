using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
    class Bullet : GameObject
    {
        private static float density = 0.01f;
        private static float radius = 0.05f;

        public Bullet(World world, Vector2 position) { 
            spriteName = "puck";

			physicsProperties.body = BodyFactory.CreateCircle(world, radius, density);
            physicsProperties.body.Restitution = 0.2f;
            physicsProperties.body.BodyType = BodyType.Dynamic;
            physicsProperties.body.Friction = 0.05f;
            physicsProperties.body.AngularDamping = 4f;

            physicsProperties.body.IgnoreGravity = true;

        }
    }
}

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
    class Panel : GameObject
    {
        public Panel(World world, Vector2 position)
		{
			this.world = world;
			spriteName = "panel";
            physicsProperties.body = BodyFactory.CreateRectangle(world, 2f, 6f, 1.0f, position);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Dynamic;
            physicsProperties.body.IgnoreGravity = true;
			physicsProperties.body.Friction = 0.0f;
			physicsProperties.body.AngularDamping = 0f;
            physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat3;
            physicsProperties.body.CollisionCategories = Category.Cat2;
            physicsProperties.body.UserData = this;

		}
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace PhysicsDefense.GameState
{
    class SampleTower : GameObject
    {

        public SampleTower(World world, Vector2 position) 
        {
            this.world = world;
            physicsProperties.body = BodyFactory.CreateRectangle(world, 0.5f, 0.5f, 1.0f, position);
            physicsProperties.body.Restitution = 0.2f;
            physicsProperties.body.BodyType = BodyType.Static;
            physicsProperties.body.IgnoreGravity = true;
            physicsProperties.body.Friction = 0.0f;
            physicsProperties.body.AngularDamping = 0f;
            physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat3 | Category.Cat4 | Category.Cat5;
            physicsProperties.body.CollisionCategories = Category.Cat7;
            physicsProperties.body.UserData = this;
        }

        public bool isSelected(MouseState state) { 
            
            if(state.LeftButton == ButtonState.Pressed
                    // state.LeftButton == ButtonState.Released
                    && state.X/GameWorld.worldScale <= position.X+0.25f
                    && state.X/GameWorld.worldScale >= position.X-0.25f
                    && state.Y/GameWorld.worldScale <= position.Y+0.25f
                    && state.Y/GameWorld.worldScale >= position.Y-0.25f)
            {
                return true;
            }

            else
                return false;
        }


    }
}

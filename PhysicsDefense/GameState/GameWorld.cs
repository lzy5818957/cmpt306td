using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PhysicsDefense.Physics;
using PhysicsDefense.Graphics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace PhysicsDefense.GameState
{
	public class GameWorld
	{
		public static float worldWidth;
		public static float worldHeight;

		PhysicsDefense game;
		PhysicsSystem physics;

		MouseState prevMouseState;

		List<GameObject> entities;
		List<GameObject> newEntities;
        List<GameObject> removeList;

		public GameWorld(PhysicsDefense game)
		{
			this.game = game;
			worldWidth = GraphicsEngine.screenWidth / PhysicsDefense.worldScale;
			worldHeight = GraphicsEngine.screenHeight / PhysicsDefense.worldScale;

			physics = new PhysicsSystem();
			entities = new List<GameObject>();
			newEntities = new List<GameObject>();
            removeList = new List<GameObject>();
		}

		public void Update(GameTime gameTime)
		{
			MouseState curMouseState = Mouse.GetState();
			bool mouseLeftPress = (curMouseState.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released);
			bool mouseRightPress = (curMouseState.RightButton == ButtonState.Pressed) && (prevMouseState.RightButton == ButtonState.Released);
			bool mouseMidPress = (curMouseState.MiddleButton == ButtonState.Pressed) && (prevMouseState.MiddleButton == ButtonState.Released);

			// Temporary for fun: adding torque to marbles
			if (Keyboard.GetState().IsKeyDown(Keys.T)) {
				foreach (GameObject obj in entities) {
					obj.physicsProperties.fixture.Body.ApplyTorque(10000);
				}
			}

			// Temporary for testing
			if (mouseMidPress) {
				Marble m = EnemyFactory.createMarble(new Vector2(Mouse.GetState().X / PhysicsDefense.worldScale, Mouse.GetState().Y / PhysicsDefense.worldScale), physics);
				addObject(m);
			}
            if (mouseLeftPress) {
				//Box b = EnemyFactory.createBox(new Vector2(Mouse.GetState().X / PhysicsDefense.worldScale, Mouse.GetState().Y / PhysicsDefense.worldScale), physics);                
				Tower t = new Tower(physics.world, new Vector2(Mouse.GetState().X / PhysicsDefense.worldScale, Mouse.GetState().Y / PhysicsDefense.worldScale));
                addObject(t);
                t.physicsProperties.fixture.Body.Rotation = (float)((curMouseState.ScrollWheelValue / 120) * Math.PI / 12.0f);
            }
			if (mouseRightPress) {
			}

            //Direction indicator
            //rotationIndicator.physicsProperties.fixture.Body.Rotation = (float)((currScrollWheelValue / 120) * Math.PI / 12.0f);
            //rotationIndicator.physicsProperties.fixture.CollisionFilter.CollidesWith = Category.None;

			// Update physics
			physics.Update(gameTime);

            // Delete the removed object in the removeList
            foreach (GameObject obj in removeList)
            {
                obj.physicsProperties.fixture.Dispose();
                entities.Remove(obj);
                game.graphics.removeObject(obj);
            }
            removeList.Clear();

			// Update game objects
			foreach (GameObject obj in entities) {
				// Check for marbles that have reached bottom
				if ((obj is Marble) && (obj.position.Y > worldHeight)) {
					removeObject(obj);

                    Explode explode=new Explode(physics.world,obj.position);
					addObject(explode);
                    game.audio.PlaySound("explode");
                }

				// Update object
				obj.update();
			}

			// Remove all dead objects
			entities.RemoveAll(delegate(GameObject obj) { return obj.isDead; });

			// Move any new objects to the main list
			entities.AddRange(newEntities);
			newEntities.Clear();

			prevMouseState = Mouse.GetState();
		}

		private void addObject(GameObject obj)
		{
			newEntities.Add(obj);
			game.graphics.addObject(obj);
			physics.addPhysical(obj);
		}

		private void removeObject(GameObject obj)
		{
			obj.isDead = true;
			game.graphics.removeObject(obj);
			physics.removePhysical(obj);
		}
	}
}

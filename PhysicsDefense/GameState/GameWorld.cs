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
		public static float worldScale = 100f;
		public static float worldWidth;
		public static float worldHeight;

		private float connectDistance = 1.0f;

		PhysicsDefense game;
		PhysicsSystem physics;

		MouseState mouseState;
		MouseState prevMouseState;
		KeyboardState keyboardState;
		KeyboardState prevKeyboardState;

		bool mouseLeftPress;
		bool mouseMidPress;
		bool mouseRightPress;

		List<GameObject> entities;
		List<GameObject> newEntities;

		Tower previewTower;

		public GameWorld(PhysicsDefense game)
		{
			this.game = game;
			worldWidth = GraphicsEngine.screenWidth / worldScale;
			worldHeight = GraphicsEngine.screenHeight / worldScale;

			physics = new PhysicsSystem();
			entities = new List<GameObject>();
			newEntities = new List<GameObject>();
		}

		private void getInputState()
		{
			mouseState = Mouse.GetState();
			keyboardState = Keyboard.GetState();
			mouseLeftPress = (mouseState.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released);
			mouseRightPress = (mouseState.RightButton == ButtonState.Pressed) && (prevMouseState.RightButton == ButtonState.Released);
			mouseMidPress = (mouseState.MiddleButton == ButtonState.Pressed) && (prevMouseState.MiddleButton == ButtonState.Released);
		}

		private void towerSelection()
		{
			if (keyboardState.IsKeyDown(Keys.T) && previewTower == null) {
				previewTower = new Tower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
				previewTower.physicsProperties.fixture.Body.Rotation = (float)((mouseState.ScrollWheelValue / 120) * Math.PI / 12.0f);
				addObject(previewTower);
			}
		}

		private void placeTower()
		{
			if (previewTower == null)
				return;

			previewTower.activate();
			addObject(previewTower);

			// Find nearby towers to connect to
			foreach (GameObject tower in entities) {
				if ((tower.position - previewTower.position).Length() > connectDistance)
					continue;
				
				// Connect the towers

			}

			previewTower = null;
		}

		public void Update(GameTime gameTime)
		{
			// Get input state
			getInputState();

			// Check for tower placement activation
			towerSelection();

			// Temporary for fun: adding torque to marbles
			if (Keyboard.GetState().IsKeyDown(Keys.S)) {
				foreach (GameObject obj in entities) {
					obj.physicsProperties.fixture.Body.ApplyTorque(1000);
				}
			}

			// Show tower preview if in tower placement mode
			if (previewTower != null) {  // TODO: make this check if in tower placement
				previewTower.position = new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale);
				previewTower.rotation = (float)((mouseState.ScrollWheelValue / 120) * Math.PI / 12.0f);
			}

			// Temporary for testing
			if (mouseMidPress) {
				Marble m = EnemyFactory.createMarble(new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale), physics);
				addObject(m);
			}
            if (mouseLeftPress && previewTower != null) {
				placeTower();
            }

			// Update physics
			physics.Update(gameTime);

			// Update game objects
			foreach (GameObject obj in entities) {
				// Update object
				obj.update();

				// Check for marbles that have reached bottom
				if ((obj is Marble) && (obj.position.Y > worldHeight)) {
					removeObject(obj);

                    Explode explode=new Explode(physics.world,obj.position);
					addObject(explode);
                    game.audio.PlaySound("explode");
                }

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

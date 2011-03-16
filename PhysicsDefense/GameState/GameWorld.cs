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

		// Convenience accessors for mouse buttons
		private bool mouseLeftPress;
		private bool mouseMidPress;
		private bool mouseRightPress;

		MapObstacles map;
		List<GameObject> entities;
		List<Tower> towers;
		List<GameObject> newEntities;

		Tower previewTower;

		public GameWorld(PhysicsDefense game)
		{
			this.game = game;
			worldWidth = GraphicsEngine.screenWidth / worldScale;
			worldHeight = GraphicsEngine.screenHeight / worldScale;

			physics = new PhysicsSystem();
			entities = new List<GameObject>();
			towers = new List<Tower>();
			newEntities = new List<GameObject>();
		}

		public void LoadContent()
		{
			Texture2D obstacles = game.Content.Load<Texture2D>("map1");
			map = new MapObstacles(physics.world, obstacles);
			game.graphics.addBackground(obstacles);
		}

		private void getInputState()
		{
			mouseState = Mouse.GetState();
			keyboardState = Keyboard.GetState();
			mouseLeftPress = (mouseState.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released);
			mouseRightPress = (mouseState.RightButton == ButtonState.Pressed) && (prevMouseState.RightButton == ButtonState.Released);
			mouseMidPress = (mouseState.MiddleButton == ButtonState.Pressed) && (prevMouseState.MiddleButton == ButtonState.Released);
		}

		/// <summary>
		/// Check if a tower was selected to prepare for placement.
		/// </summary>
		private void towerSelection()
		{
			if (previewTower != null)
				return;

			if (keyboardState.IsKeyDown(Keys.T))
				previewTower = new Tower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));

			if (previewTower != null)
				addObject(previewTower);
		}

		/// <summary>
		/// Place a tower by activating the preview tower.
		/// </summary>
		private void placeTower()
		{
			if (previewTower == null)
				return;

			// Don't allow placement on top of other towers
			//if (previewTower.collisionCount > 0)
			if (previewTower.isColliding)
				return;

			// Find nearby towers to connect to
			foreach (Tower tower in towers) {
				float distance = (tower.position - previewTower.position).Length();
				if (distance > connectDistance || tower == previewTower)
					continue;

				// Change the nearby towers back from green to their normal color since they were set to green right before this
				tower.color = tower.nativeColor;

				// Connect the towers
				Connector con = new Connector(physics.world, distance, tower.size.X);
				con.position = (tower.position + previewTower.position) / 2f;
				con.rotation = (float)Math.Atan2((tower.position.Y - previewTower.position.Y), (tower.position.X - previewTower.position.X));
				addObject(con);
			}

			// Activate the preview tower so it becomes a real, solid new tower, and add it to the world
			previewTower.activate();
			previewTower = null;
		}

		/// <summary>
		/// Drew the preview tower and indicate connections/collisions.
		/// </summary>
		private void showPreviewTower()
		{
			if (previewTower == null)
				return;

			previewTower.position = new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale);

			//if (previewTower.collisionCount > 0) {
			if (previewTower.isColliding) {
				previewTower.color = Color.Red;
				previewTower.color.A = 128;
			} else {
				previewTower.color = Color.White;
				previewTower.color.A = 128;
				foreach (Tower tower in towers) {
					if (tower == previewTower)
						continue;
					if ((tower.position - previewTower.position).Length() < connectDistance)
						tower.color = Color.GreenYellow;
					else
						tower.color = tower.nativeColor;
				}
			}
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
					obj.physicsProperties.body.ApplyTorque(1000);
				}
			}

			// Temporary for testing
			if (mouseRightPress) {
				Marble m = EnemyFactory.createMarble(new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale), physics);
				addObject(m);
			}

			// Show tower preview if in tower placement mode
			if (previewTower != null) {
				showPreviewTower();

				// Place tower if mouse clicked
				if (mouseLeftPress)
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
					obj.die();

					Explode explode = new Explode(physics.world, obj.position);
					addObject(explode);
				}

			}

			// Remove all dead objects
			entities.RemoveAll(delegate(GameObject obj) { return obj.isDead; });
			towers.RemoveAll(delegate(Tower obj) { return obj.isDead; });

			// Move any new objects to the main list
			entities.AddRange(newEntities);
			newEntities.Clear();

			prevMouseState = Mouse.GetState();
			prevKeyboardState = Keyboard.GetState();
		}

		private void addObject(GameObject obj)
		{
			obj.onPlaySound = game.audio.PlaySound;
			obj.onDeath = removeObject;
			obj.initialize();

			newEntities.Add(obj);
			if (obj is Tower)
				towers.Add((Tower)obj);

			game.graphics.addObject(obj);
			physics.addPhysical(obj);
		}

		private void removeObject(GameObject obj)
		{
			game.graphics.removeObject(obj);
			physics.removePhysical(obj);
		}
	}
}

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

		private bool active = true;
		private float spinDirection = 1f;

		private String currentMap;
		private float connectDistance = 1.0f;
		private float clickSpinTorque = 200f;

		private float money = 100;
		private int lives = 20;

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
		EnemyEmitter spawner;

		List<GameObject> entities;
		List<Tower> towers;
		List<Marble> enemies;
        List<Bullet> bullets;
        List<Missile> missiles;
		List<GameObject> newEntities;

		Tower previewTower;
        Tower currentTower;
        SampleTower sampleTower;

		public GameWorld(PhysicsDefense game)
		{
			this.game = game;
			worldWidth = GraphicsEngine.screenWidth / worldScale;
			worldHeight = GraphicsEngine.screenHeight / worldScale;

			physics = new PhysicsSystem();
			entities = new List<GameObject>();
			enemies = new List<Marble>();
			towers = new List<Tower>();
			newEntities = new List<GameObject>();
            bullets = new List<Bullet>();
            missiles = new List<Missile>();
		}

		public void LoadContent()
		{
			Texture2D obstacles = game.Content.Load<Texture2D>(ResourceManager.initialMap);
			map = new MapObstacles(physics.world, obstacles);
            game.graphics.addBackground("pictures/backgrounds/gamescreenbackground");
            game.graphics.addBackground(obstacles);
            currentMap = ResourceManager.initialMap;

			spawner = new EnemyEmitter(new Vector2(1f, 0f), 1);
			spawner.onSpawn = spawnEnemy;
			spawner.onWaveFinished = waveFinished;
			spawner.start();
            initPanel();
		}


        private void initPanel()
        {
            Panel m = new Panel(physics.world, new Vector2(9, 3));
            addObject(m);
            sampleTower = new SampleTower(physics.world, new Vector2(8.4f, 2));
            addObject(sampleTower);
        }

		private void spawnEnemy(EnemyType enemy)
		{
			Marble m = new Marble(physics.world, spawner.position, ((spawner.wave - 1) * WaveData.healthMult) + 1f, ((spawner.wave - 1) * WaveData.bountyMult) + 1f);
			addObject(m);
		}


		private void waveFinished()
		{
			Console.WriteLine("Wave " + spawner.wave + " finished\n"+"LIVE(S)="+lives);
            MessageBoard.updateMessage("Wave " + spawner.wave + " finished\n" + "LIVE(S)=" + lives);

			// Give wave money reward
			money += 50 + (spawner.wave * 10);

			spawner.nextWave();
			spawner.start();
		}

		public void lose()
		{
			Console.WriteLine("All lives lost!");
            MessageBoard.updateMessage("*** All lives lost! ***\nGame Over");
			active = false;
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

            if (keyboardState.IsKeyDown(KeyBindings.placeBasicTower) || sampleTower.isSelected(mouseState))
            {
                previewTower = new BasicTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
            }else if(keyboardState.IsKeyDown(KeyBindings.placeMissileTower))
            {
                previewTower = new MissileTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
            }
            else if (keyboardState.IsKeyDown(KeyBindings.placeHeroTower))
            {
                previewTower = new HeroTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
            }
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
			
			// Check if sufficient money is available
			float cost = Tower.cost;
			if (money < cost) {
				MessageBoard.updateMessage("Insufficient funds\n to place tower");
				return;
			}

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
			money -= cost;
		}

		/// <summary>
		/// Drew the preview tower and indicate connections/collisions.
		/// </summary>
		private void showPreviewTower()
		{
			if (previewTower == null)
				return;

			previewTower.position = new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale);

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

		public void spinClick()
		{
			// Check for tower spin-click
			if (mouseLeftPress)
				spinDirection = -1f;
			if (mouseRightPress)
				spinDirection = 1f;

			if (keyboardState.IsKeyDown(KeyBindings.spinMod) && (mouseLeftPress || mouseRightPress)) {
				Vector2 clickPos = new Vector2(mouseState.X / worldScale, mouseState.Y / worldScale);
				foreach (Tower tower in towers) {
					if ((clickPos - tower.position).Length() < tower.radius) {
						tower.applySpin(clickSpinTorque * spinDirection);
					}
				}
			}
		}

        public void towerOperation() {
            foreach (Tower tower in towers)
            {
                if (tower.isSelected(mouseState))
                {
                    currentTower = tower;
                    continue;
                }
            }        
        }

		public void loseLife()
		{
			lives--;
			MessageBoard.updateMessage("LIVE(S)=" + lives);
		}

		public void Update(GameTime gameTime)
		{
			if (!active)
				return;

			// Get input state
			getInputState();

			if (keyboardState.IsKeyDown(KeyBindings.cancelTower) && previewTower != null) {
				// Cancel tower placement
				removeObject(previewTower);
				previewTower = null;
			}

			// Check for tower placement activation
			towerSelection();

			// Check for spin clicks
			spinClick();

			if (Keyboard.GetState().IsKeyDown(KeyBindings.spin)) {
				foreach (Tower tower in towers) {
					if(tower.isActivated)
						tower.applySpin(100f);
				}
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

			// Update enemy spawners
			spawner.update(gameTime);

			// Update game objects
			foreach (GameObject obj in entities) {
				// Update object
				obj.update(gameTime);

				// Check for marbles that have reached bottom
				if ((obj is Marble) && (obj.position.Y > worldHeight)) {
					((Marble)obj).diedByStuck = true;
					obj.die();
				}
   			}

			// Remove all dead objects
			entities.RemoveAll(delegate(GameObject obj) { return obj.isDead; });
			towers.RemoveAll(delegate(Tower obj) { return obj.isDead; });
            bullets.RemoveAll(delegate(Bullet obj) { return obj.isDead; });
            missiles.RemoveAll(delegate(Missile obj) { return obj.isDead; });
			enemies.RemoveAll(delegate(Marble obj) { return obj.isDead; });

			// Move any new objects to the main list
			entities.AddRange(newEntities);
			newEntities.Clear();

			// Check if all lives were lost
			if (lives <= 0) {
				lose();
			}

			// Check if ready to go to the next wave
			if (!spawner.active && spawner.waveFinished && enemies.Count <= 0) {
				waveFinished();
			}

			prevMouseState = Mouse.GetState();
			prevKeyboardState = Keyboard.GetState();
		}

		private void addObject(GameObject obj)
		{
			obj.onPlaySound = game.audio.PlaySound;
			obj.onDeath = removeObject;
			obj.onCreateObject = addObject;
			obj.initialize();

			newEntities.Add(obj);
			if (obj is Tower)
				towers.Add((Tower)obj);
			if (obj is Marble)
				enemies.Add((Marble)obj);
            if (obj is Bullet)
                bullets.Add((Bullet)obj);
            if (obj is Missile)
                missiles.Add((Missile)obj);

			game.graphics.addObject(obj);
			physics.addPhysical(obj);
		}

		private void removeObject(GameObject obj)
		{
			// If this is a marble that was shot down, award money
			if (obj is Marble) {
				Marble m = (Marble)obj;

				if (m.diedByStuck) {
					loseLife();
				} else {
					money += m.bounty;
				}
			}

			if (obj is Tower)
				towers.Remove((Tower)obj);
			if (obj is Marble)
				enemies.Remove((Marble)obj);
			if (obj is Bullet)
				bullets.Remove((Bullet)obj);
			if (obj is Missile)
				missiles.Remove((Missile)obj);
			game.graphics.removeObject(obj);
			physics.removePhysical(obj);
		}
	}
}

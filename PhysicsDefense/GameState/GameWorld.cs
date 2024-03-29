﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PhysicsDefense.Physics;
using PhysicsDefense.Graphics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using GameStateManagement;



namespace PhysicsDefense.GameState
{
	public class GameWorld
	{
		public static float worldScale = 100f;
		public static float worldWidth;
		public static float worldHeight;
        public static float connectDistance = 1.0f;
        private bool started = false;
		private bool active = true;
		private float spinDirection = 1f;

		private String currentMap;
		private float clickSpinTorque = 200f;

		public static float money = 400;

		private int lives = 20;

		PhysicsDefense game;
		PhysicsSystem physics;

		public static MouseState mouseState;
		MouseState prevMouseState;
		KeyboardState keyboardState;
		KeyboardState prevKeyboardState;

		// Convenience accessors for mouse buttons
		public static bool mouseLeftPress;
        public static bool mouseMidPress;
        public static bool mouseRightPress;

		MapObstacles map;
		EnemyEmitter spawner;
        Panel panelR;
        Panel panelL;

		List<GameObject> entities;
		List<Tower> towers;
		List<Marble> enemies;
        List<Bullet> bullets;
        List<Missile> missiles;
		List<GameObject> newEntities;

		Tower previewTower;
        Tower currentTower;
        SampleBTower sampleBTower;
        SampleMTower sampleMTower;
        SampleHTower sampleHTower;

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
			
            started = false;

            initPanel();
            
		}

        private void initPanel()
        {
            panelR = new Panel(physics.world, new Vector2(9, 3));
            addObject(panelR);

            panelL = new Panel(physics.world, new Vector2(-1, 3));
            addObject(panelL);

            sampleBTower = new SampleBTower(physics.world, new Vector2(8.4f, 2.05f));
            addObject(sampleBTower);
            sampleMTower = new SampleMTower(physics.world, new Vector2(9.0f, 2.05f));
            addObject(sampleMTower);
            sampleHTower = new SampleHTower(physics.world, new Vector2(9.6f, 2.05f));
            addObject(sampleHTower);
            active = true;
        }

		private void spawnEnemy(EnemyType enemy)
		{
			Marble m = new Marble(physics.world, spawner.position, ((spawner.wave - 1) * WaveData.healthMult) + 1f, ((spawner.wave - 1) * WaveData.bountyMult) + 1f);
			addObject(m);
		}


		private void waveFinished()
		{
            panelR.playSound("win");
            InfoBoard.updateInfo("Wave " + spawner.wave + " finished\n" + "Press Space for the next wave",Color.SkyBlue,500);

			// Give wave money reward
			money += 20*((int)(spawner.wave/10) +1);

			spawner.nextWave();
			spawner.active = false;
			started = false;
			//spawner.start();
		}

		public void lose()
		{
            panelR.playSound("lose");
            InfoBoard.updateInfo("All lives lost!\nPress F10 to bring up the menu.",Color.Red,65535);
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
            {
                InfoBoard.updateInfo("Select Location \nPress Esc to cancel.", Color.PaleGreen, 1);
                return;
            }

            if (keyboardState.IsKeyDown(KeyBindings.placeBasicTower) || sampleBTower.isSelected(mouseState))
            {

                previewTower = new BasicTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
            }
            else if (keyboardState.IsKeyDown(KeyBindings.placeMissileTower) || sampleMTower.isSelected(mouseState))
            {
                previewTower = new MissileTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));

            }
            else if (keyboardState.IsKeyDown(KeyBindings.placeHeroTower) || sampleHTower.isSelected(mouseState))
            {
                previewTower = new HeroTower(physics.world, new Vector2(Mouse.GetState().X / worldScale, Mouse.GetState().Y / worldScale));
            }
            if (previewTower != null)
            {
                // Check if sufficient money is available
                float cost = Tower.cost;
                if (money < cost)
                {
                    InfoBoard.updateInfo("Insufficient funds to place this tower", Color.Tomato, 200);
                    panelR.playSound("error");
                    // Cancel tower placement
                    foreach (Tower tower in towers)
                    {
                        tower.color = tower.nativeColor;
                    }
                    previewTower = null;
                }else{
                    addObject(previewTower);
                    deactivateMenu();
                }
            }
		}

        private void deactivateMenu()
        {
            sampleBTower.color=Color.Transparent;
            sampleHTower.color = Color.Transparent;
            sampleMTower.color = Color.Transparent;
        }

        private void activateMenu()
        {
            sampleBTower.color=sampleBTower.nativeColor;
            sampleHTower.color = sampleBTower.nativeColor;
            sampleMTower.color = sampleBTower.nativeColor;
        }

		/// <summary>
		/// Place a tower by activating the preview tower.
		/// </summary>
		private void placeTower()
		{
            if (previewTower == null)
            {
                return;
            }

			// Don't allow placement on top of other towers
			//if (previewTower.collisionCount > 0)
			if (previewTower.isColliding)
				return;
			
			// Check if sufficient money is available
			float cost = Tower.cost;
			if (money < cost) {
				InfoBoard.updateInfo("Insufficient funds to place this tower",Color.Tomato,300);
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
				Connector con = new Connector(physics.world, distance, tower.size.X,previewTower,tower);
                addObject(con);
			}

			// Activate the preview tower so it becomes a real, solid new tower, and add it to the world
			previewTower.activate();
			previewTower = null;
			money -= cost;

            //Rest Panel Tower
            activateMenu();
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
                        panelR.playSound("spin");
                        break;
					}
				}
			}
		}

        public void towerOperation() {

            bool isAnyTowerSelectedAtAll = false;
            foreach (Tower tower in towers)
            {
                tower.checkSelected(mouseState);
                if (tower.isSelected)
                {
                    if (currentTower != null)
                    {
                        currentTower.color = tower.nativeColor;
                        
                    }
                    currentTower = tower;
                    isAnyTowerSelectedAtAll = true;
                    panelR.playSound("click");
                }
            }
            if (!isAnyTowerSelectedAtAll && currentTower != null && mouseState.X < 800)
            {
                currentTower.color = currentTower.nativeColor;
                currentTower = null;
            }

            if (currentTower != null)
            {
                currentTower.color = Color.DimGray;
            }
            MessageBoard.updateMenu(currentTower);
        }

		public void loseLife()
		{
			lives--;
			Console.WriteLine("LIVE(S)=" + lives);
		}

		public void Update(GameTime gameTime)
		{
			if (!active)
				return;
            
            // Get input state
            getInputState();

            if (keyboardState.IsKeyDown(KeyBindings.showHelp))
            {
                //InfoBoard.updateInfo("HOT KEYS\n"+
                //                     "T=Basic Tower\n"+
                //                     "M=Missile Tower\n"+
                //                     "H=Hero Tower\n"+
                //                     "R=Show range indicator"+
                //                     "F10=Pause",Color.Black,2000);
                InfoBoard.updateInfo("Enemies spawn at the top left of the screen and come in waves.\nThey fall under gravity and roll across terrain/towers.\nLose a life if they get stuck or fall to the bottom.\nHero towers level up and are upgradable.\nHold shift and left/right click towers to spin.\nTowers make 'bridges' if placed close to each other.\n\n" +
                    "Hotkeys: T=Basic tower\nM=Missile tower\nH=Hero tower\nR=Range indicator\nEscape=Cancel tower placement\nF10=Menu", Color.White, 10000);

            }

            if (keyboardState.IsKeyDown(KeyBindings.startGame))
            {
                if (!started)
                {
                    spawner.start();
                    started = true;
                    InfoBoard.updateInfo("Wave " + spawner.wave + " is coming!", Color.Orange, 200);
                    panelR.playSound("wavestart");
                    //onPlaySound("wavestart");
                }
            }

			if (keyboardState.IsKeyDown(KeyBindings.cancelTower)) {
				
                if (previewTower == null)
                {
 
                }
                else
                {
                    // Cancel tower placement
                    foreach (Tower tower in towers)
                    {
                        tower.color = tower.nativeColor;
                    }
                    activateMenu();
                    previewTower.die();
                    //removeObject(previewTower);
                    previewTower = null;
                }
			}

            //Check if there is a need to show all Range Indicators
            if (keyboardState.IsKeyDown(KeyBindings.showRangeIndicators))
                RangeIndicator.isVisibleAll = true;
            else
                RangeIndicator.isVisibleAll = false;

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
                {
                    placeTower();
                }
			}

			// Check for tower placement activation
			// This must be after the tower placement check.
			towerSelection();

			// Update physics
			physics.Update(gameTime);

			// Update enemy spawners
			spawner.update(gameTime);

			// Update game objects
			foreach (GameObject obj in entities) {
				// Update object
                if(!obj.isDead)
				    obj.update(gameTime);

                // Check for marbles that have reached bottom
                if ((obj is Marble) && (obj.position.Y > worldHeight))
                {
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
				spawner.waveFinished = false;
			}

			prevMouseState = Mouse.GetState();
			prevKeyboardState = Keyboard.GetState();

            // Place tower if mouse clicked
            if (mouseLeftPress && !Keyboard.GetState().IsKeyDown(KeyBindings.spinMod))
            {
                towerOperation();
            }
            MessageBoard.updateMessage(money + "\n" + lives);
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

        public void clearObj() {

            foreach (GameObject obj in entities)
            {
                    obj.die();
            }
            foreach (Tower tower in towers)
            {
                tower.die();
            }
            foreach (Marble marble in enemies)
            {
                marble.die();
            }
            foreach (Bullet bullet in bullets)
            {
                bullet.die();
            }
            foreach (Missile missile in missiles)
            {
                missile.die();
            }
            foreach (GameObject newObj in newEntities)
            {
                newObj.die();
            }
            money = 1500;
            lives = 20;
        }
	}
}

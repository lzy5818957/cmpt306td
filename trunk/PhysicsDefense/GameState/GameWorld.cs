﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PhysicsDefense.Physics;
using PhysicsDefense.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDefense.GameState
{
	class GameWorld
	{
		PhysicsSystem physics;
	    GraphicsEngine graphics;

        Boolean Lpressed, Lclicked, Rpressed, Rclicked;
        int currScrollWheelValue;
        //Box rotationIndicator;

		List<GameObject> entities;
        public List<GameObject> removeList;
        List<Explode> explodeList;

		public GameWorld(GraphicsEngine graphics)
		{
			this.graphics = graphics;
			physics = new PhysicsSystem();
			entities = new List<GameObject>();
            removeList = new List<GameObject>();
            explodeList = new List<Explode>();

			// Add a temporary test enemies
			//Marble testMarble = EnemyFactory.createMarble(new Vector2(10f, 3f), physics);
			//addObject(testMarble);

			//Box testBox = EnemyFactory.createBox(new Vector2(10f, 24f), physics);
			//addObject(testBox);
            //testBox.physicsProperties.fixture.Body.Rotation = (float)Math.PI / 12.0f;

			//Box testBox2 = EnemyFactory.createBox(new Vector2(28.2f, 45f), physics);
			//addObject(testBox2);
			
			//Marble testMarble2 = EnemyFactory.createMarble(new Vector2(27.8f, 35f), physics);
			//addObject(testMarble2);

            //rotationIndicator = EnemyFactory.createBox(new Vector2(75f, 5f), physics);
            //addObject(rotationIndicator);
		}

		public void Update(GameTime gameTime)
		{
            MouseState mouse = Mouse.GetState();

            //ScrollWheelValue
            currScrollWheelValue = mouse.ScrollWheelValue;

			// Add Marble by mouse
            if (mouse.RightButton == ButtonState.Pressed)
                Lpressed = true;

            if (mouse.RightButton == ButtonState.Released && Lpressed)
            {
                Lclicked = true;
                Lpressed = false;
            }

            if (Lclicked)
            {
                Marble m = EnemyFactory.createMarble(new Vector2(Mouse.GetState().X / 10f, Mouse.GetState().Y / 10f), physics);
                addObject(m);
                Lclicked = false; 
            }

            // Add Box by mouse
            if (mouse.LeftButton == ButtonState.Pressed)
                Rpressed = true;

            if (mouse.LeftButton == ButtonState.Released && Rpressed)
            {
                Rclicked = true;
                Rpressed = false;
            }

            if (Rclicked)
            {
                Box b = EnemyFactory.createBox(new Vector2(Mouse.GetState().X / 10f, Mouse.GetState().Y / 10f), physics);                
                addObject(b);
                b.physicsProperties.fixture.Body.Rotation = (float)((currScrollWheelValue / 120) * Math.PI / 12.0f);

                Rclicked = false;
            }

            //Direction indicator

            //rotationIndicator.physicsProperties.fixture.Body.Rotation = (float)((currScrollWheelValue / 120) * Math.PI / 12.0f);

			// Update physics
			physics.Update(gameTime);

            foreach (Explode exp in explodeList)
            {
                addObject(exp);
            }
            explodeList.Clear();

            //Delete the removed object in the removeList
            foreach (GameObject obj in removeList)
            {
                obj.physicsProperties.fixture.Dispose();
                entities.Remove(obj);
                graphics.removeObject(obj);
            }
            removeList.Clear();

			// Update game objects
			foreach (GameObject obj in entities) {

                if (obj.position.Y > 50&& (obj is Marble) )
                {
                    removeList.Add(obj);
                    Explode explode=new Explode(physics.world,obj.position);
                    explodeList.Add(explode);
                    graphics.explodeSound.Play();
                }

                if (obj is Explode)
                    if (((Explode)obj).frameCount >= 17)
                        removeList.Add(obj);

				obj.update();
                if (obj.position.X + obj.size.X < 0
                    || obj.position.Y + obj.size.Y < 0
                    || obj.position.X - obj.size.X > graphics.device.PreferredBackBufferWidth/10
                    || obj.position.Y - obj.size.Y > graphics.device.PreferredBackBufferHeight/10)
                {
                    removeList.Add(obj);
                }

			}
		}

		private void addObject(GameObject obj)
		{
			entities.Add(obj);
			graphics.addObject(obj);
			physics.addPhysical(obj);
		}
	}
}
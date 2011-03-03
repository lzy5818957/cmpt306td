using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using PhysicsDefense.Graphics;

namespace PhysicsDefense.GameState
{
	class GameWorld
	{
		PhysicsSystem physics;
		GraphicsEngine graphics;

		List<GameObject> entities;

		public GameWorld(GraphicsEngine graphics)
		{
			this.graphics = graphics;
			physics = new PhysicsSystem();
			entities = new List<GameObject>();

			// Add a temporary test enemy
			Marble testMarble = EnemyFactory.createMarble(new Vector2(100f, 30f), physics);
			addObject(testMarble);
			Box testBox = EnemyFactory.createBox(new Vector2(100f, 240f), physics);
			addObject(testBox);
			Box testBox2 = EnemyFactory.createBox(new Vector2(200f, 300f), physics);
			testBox.physicsProperties.fixture.Body.Rotation = 1f;
		}

		public void Update(GameTime gameTime)
		{
			// Update physics
			physics.Update(gameTime);

			// Update game objects
			foreach (GameObject obj in entities) {
				obj.update();
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

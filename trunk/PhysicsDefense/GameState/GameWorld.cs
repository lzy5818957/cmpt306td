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
			Marble testMarble = EnemyFactory.createMarble(new Vector2(10f, 3f), physics);
			addObject(testMarble);
			Box testBox = EnemyFactory.createBox(new Vector2(10f, 24f), physics);
			addObject(testBox);
			Box testBox2 = EnemyFactory.createBox(new Vector2(28.2f, 45f), physics);
			addObject(testBox2);
			testBox.physicsProperties.fixture.Body.Rotation = (float)Math.PI / 12.0f;
			Marble testMarble2 = EnemyFactory.createMarble(new Vector2(27.8f, 35f), physics);
			addObject(testMarble2);
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

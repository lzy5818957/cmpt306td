using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using PhysicsDefense.GameState;

namespace PhysicsDefense.Physics
{
	class PhysicsSystem
	{
		List<GameObject> physicalObjects;

		public World world { get; protected set; }

		public PhysicsSystem()
		{
			physicalObjects = new List<GameObject>();
			world = new World(new Vector2(0, 9.81f));
		}

		public void Update(GameTime gameTime)
		{
			// Perform physics step
			float timeStep = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;
			//float timeStep = 1f / 60f;
			world.Step(timeStep);
		}

		public void addPhysical(GameObject obj)
		{
			physicalObjects.Add(obj);
		}

		public void removePhysical(GameObject obj)
		{
			physicalObjects.Remove(obj);
		}
	}
}

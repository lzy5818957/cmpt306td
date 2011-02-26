using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.Physics
{
	class PhysicsSystem
	{
		World world;

		public PhysicsSystem()
		{
			world = new World(new Vector2(0, -9.81f));
		}

		public void Update(GameTime gameTime)
		{
			world.Step(1f / 60f);
		}
	}
}

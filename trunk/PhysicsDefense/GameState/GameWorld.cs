using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;

namespace PhysicsDefense.GameState
{
	class GameWorld
	{
		PhysicsSystem physics;

		public GameWorld()
		{
			physics = new PhysicsSystem();
		}

		public void Update(GameTime gameTime)
		{
			physics.Update(gameTime);
		}
	}
}

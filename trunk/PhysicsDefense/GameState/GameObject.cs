using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using FarseerPhysics.Collision;

namespace PhysicsDefense.GameState
{
	abstract class GameObject
	{
		//public Vector2 position { get; protected set; }
		public String spriteName { get; protected set; }
		public ObjectPhysicsProperties physicsProperties { get; protected set; }

		public GameObject()
		{
			physicsProperties = new ObjectPhysicsProperties();
		}

		public void update()
		{
			//position = physicsProperties.fixture.Body.Position;
		}

		public Vector2 getPosition()
		{
			return physicsProperties.fixture.Body.Position;
		}

		public Rectangle getBoundingBox()
		{
			AABB aabb;
			physicsProperties.fixture.GetAABB(out aabb, 0);
			Rectangle rect = new Rectangle(
				(int)(getPosition().X + aabb.LowerBound.X),
				(int)(getPosition().Y + aabb.LowerBound.Y),
				(int)(aabb.UpperBound.X - aabb.LowerBound.X),
				(int)(aabb.UpperBound.Y - aabb.LowerBound.Y)
			);
			return rect;
		}
	}
}

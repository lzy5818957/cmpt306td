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
		public Vector2 position { get { return physicsProperties.fixture.Body.Position; } protected set { } }
		public String spriteName { get; protected set; }
		public ObjectPhysicsProperties physicsProperties { get; protected set; }

		public GameObject()
		{
			physicsProperties = new ObjectPhysicsProperties();
		}

		public void update()
		{
		}

		public Rectangle getBoundingBox()
		{
			AABB aabb;
			physicsProperties.fixture.GetAABB(out aabb, 0);
			//Rectangle rect = new Rectangle(
			//	(int)(position.X + aabb.LowerBound.X),
			//	(int)(position.Y + aabb.LowerBound.Y),
			//	(int)(aabb.UpperBound.X - aabb.LowerBound.X),
			//	(int)(aabb.UpperBound.Y - aabb.LowerBound.Y)
			//);
			Rectangle rect = new Rectangle(
				(int)aabb.LowerBound.X,
				(int)aabb.LowerBound.Y,
				(int)aabb.Extents.X * 2,
				(int)aabb.Extents.Y * 2
			);
			return rect;
		}
	}
}

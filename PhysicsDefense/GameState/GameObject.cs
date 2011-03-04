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
		public float rotation { get { return physicsProperties.fixture.Body.Rotation; } protected set { } }
		public String spriteName { get; protected set; }
		public ObjectPhysicsProperties physicsProperties { get; protected set; }

		public Vector2 size {
			get {
				AABB aabb;
				physicsProperties.fixture.GetAABB(out aabb, 0);
				return new Vector2(aabb.Extents.X * 2, aabb.Extents.Y * 2);
			}
			set {
			}
		}

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
			Rectangle rect = new Rectangle(
				(int)(position.X - aabb.Extents.X),
				(int)(position.Y - aabb.Extents.Y),
				(int)(aabb.Extents.X * 2),
				(int)(aabb.Extents.Y * 2)
			);
			
			return rect;
		}
	}
}

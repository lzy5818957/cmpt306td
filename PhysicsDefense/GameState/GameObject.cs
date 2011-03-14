using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDefense.GameState
{
	public abstract class GameObject
	{
		public Vector2 position {
			get { return physicsProperties.fixture.Body.Position; }
			set { physicsProperties.fixture.Body.SetTransformIgnoreContacts(ref value, physicsProperties.fixture.Body.Rotation); }
		}

		public float rotation {
			get { return physicsProperties.fixture.Body.Rotation; }
			set { physicsProperties.fixture.Body.Rotation = value; }
		}

		public String spriteName { get; protected set; }
		public Color nativeColor = Color.White;
		public Color color;

		public ObjectPhysicsProperties physicsProperties { get; protected set; }
		public bool isColliding = false;

		public bool isDead = false;
		public bool isEnabled = true;

		public Vector2 size {
			get {
				AABB aabb;
				physicsProperties.fixture.GetAABB(out aabb, 0);
				return new Vector2(aabb.Extents.X * 2, aabb.Extents.Y * 2);
			}
			set { }
		}

		public GameObject()
		{
			physicsProperties = new ObjectPhysicsProperties();
			//physicsProperties.fixture.OnCollision = (a, b, c) => { return this.isColliding = true; };
			color = nativeColor;
		}

		public virtual void activate()
		{
			color = nativeColor;
			color.A = 255;
		}

        public abstract void update();

		//public Rectangle getBoundingBox()
		//{
		//    AABB aabb;
		//    physicsProperties.fixture.GetAABB(out aabb, 0);
		//    Rectangle rect = new Rectangle(
		//        (int)(position.X - aabb.Extents.X),
		//        (int)(position.Y - aabb.Extents.Y),
		//        (int)(aabb.Extents.X * 2),
		//        (int)(aabb.Extents.Y * 2)
		//    );
			
		//    return rect;
		//}
	}
}

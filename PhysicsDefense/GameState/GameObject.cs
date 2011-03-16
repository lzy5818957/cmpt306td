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
	public delegate void SoundHandler(String soundName);
	public delegate void DeathHandler(GameObject obj);

	public abstract class GameObject
	{
		public Vector2 position {
			get { return physicsProperties.body.Position; }
			set { physicsProperties.body.SetTransformIgnoreContacts(ref value, physicsProperties.body.Rotation); }
		}

		public float rotation {
			get { return physicsProperties.body.Rotation; }
			set { physicsProperties.body.Rotation = value; }
		}

		public Vector2 size {
			get {
				AABB aabb;
				physicsProperties.body.FixtureList[0].GetAABB(out aabb, 0);
				return new Vector2(aabb.Extents.X * 2, aabb.Extents.Y * 2);
			}
			private set { }
		}

		private ulong ticks = 0;
		private ulong ticksCollision = 0;
		public bool isColliding = false;
		public ObjectPhysicsProperties physicsProperties { get; protected set; }

		public String spriteName { get; protected set; }
		public Color nativeColor = Color.White;
		public Color color;

		public bool isDead = false;
		public bool isEnabled = true;

		public SoundHandler onPlaySound;
		public DeathHandler onDeath;

		public GameObject()
		{
			physicsProperties = new ObjectPhysicsProperties();
			color = nativeColor;
		}

		public virtual void initialize()
		{
			physicsProperties.body.OnCollision += (a, b, c) => { ticksCollision = ticks; isColliding = true; return true; };
			physicsProperties.body.OnSeparation += (a, b) => { if (ticksCollision < ticks) isColliding = false; };
		}

		public virtual void die()
		{
			isDead = true;
			isEnabled = false;
			onDeath(this);
		}

		public virtual void update() {
			ticks++;
		}

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

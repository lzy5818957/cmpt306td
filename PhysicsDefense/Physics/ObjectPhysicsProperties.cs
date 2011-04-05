using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.Physics
{
	public class ObjectPhysicsProperties
	{
		public float speed
		{
			get { return body.LinearVelocity.Length(); }
			private set { }
		}
        public Vector2 velocity
        {
            get { return body.LinearVelocity; }
            private set { }
        }
		public Body body;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;

namespace PhysicsDefense.Physics
{
	public class ObjectPhysicsProperties
	{
		public float speed
		{
			get { return body.LinearVelocity.Length(); }
			private set { }
		}

		public Body body;
	}
}

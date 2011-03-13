using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;

namespace PhysicsDefense.Physics
{
	public class ObjectPhysicsProperties
	{
		public ObjectPhysicsProperties()
		{
			fixtureList = new List<Fixture>();
		}

		public Fixture fixture
		{
			get { return fixtureList[0]; }
			set { fixtureList.Insert(0, value); }
		}

		//public List<Fixture> fixture;
		public List<Fixture> fixtureList;
	}
}

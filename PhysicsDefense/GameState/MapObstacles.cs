using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.Decomposition;

namespace PhysicsDefense.GameState
{
	class MapObstacles
	{
		List<Fixture> fixtures;

		public MapObstacles(World world, Texture2D texture)
		{
			uint[] data = new uint[texture.Width * texture.Height];
			texture.GetData(data);
			Vertices verts = PolygonTools.CreatePolygon(data, texture.Width, texture.Height, true);
			//Vertices verts = PolygonTools.CreatePolygon(data, texture.Width, texture.Height, 0.01f, 16, true, true);
			Vector2 scale = new Vector2(0.01f, 0.01f);
			verts.Scale(ref scale);

			List<Vertices> decomposedVerts = BayazitDecomposer.ConvexPartition(verts);
			fixtures = FixtureFactory.CreateCompoundPolygon(world, decomposedVerts, 1f);
			fixtures[0].Body.BodyType = BodyType.Static;
		}
	}
}

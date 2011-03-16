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
		List<List<Fixture>> fixtures;

		public MapObstacles(World world, Texture2D texture)
		{
			fixtures = new List<List<Fixture>>();
			uint[] data = new uint[texture.Width * texture.Height];
			texture.GetData(data);
			List<Vertices> verts = PolygonTools.CreatePolygon(data, texture.Width, texture.Height, 1f, 16, true, false);

			foreach (Vertices poly in verts) {
				Vector2 scale = new Vector2(1f / GameWorld.worldScale, 1f / GameWorld.worldScale);
				poly.Scale(ref scale);

				List<Vertices> decomposedVerts = CDTDecomposer.ConvexPartition(poly);
				List<Fixture> fix = FixtureFactory.CreateCompoundPolygon(world, decomposedVerts, 1f);

				// Obstacle physics properties
				fix[0].Friction = 0.8f;
				fix[0].Restitution = 0.2f;
				fix[0].Body.BodyType = BodyType.Static;
				fixtures.Add(fix);
			}
		}
	}
}

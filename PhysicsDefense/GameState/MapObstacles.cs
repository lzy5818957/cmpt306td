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
		List<Body> fixtures;

		public MapObstacles(World world, Texture2D texture)
		{
			fixtures = new List<Body>();
			uint[] data = new uint[texture.Width * texture.Height];
			texture.GetData(data);
			List<Vertices> verts = PolygonTools.CreatePolygon(data, texture.Width, 0.05f, 16, true, true);

			foreach (Vertices poly in verts) {
				Vector2 scale = new Vector2(1f / GameWorld.worldScale, 1f / GameWorld.worldScale);
				poly.Scale(ref scale);

				List<Vertices> decomposedVerts = BayazitDecomposer.ConvexPartition(poly);
				var body = BodyFactory.CreateCompoundPolygon(world, decomposedVerts, 3f);

				// Obstacle physics properties
				body.Friction = 0.8f;
				body.Restitution = 0.2f;
				body.BodyType = BodyType.Static;
				fixtures.Add(body);
			}
		}
	}
}

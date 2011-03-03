using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsDefense.GameState;

namespace PhysicsDefense.Graphics
{
	class GraphicsEngine
	{
		GraphicsDeviceManager device;
		SpriteBatch spriteBatch;
		Game game;
		
		List<GameObject> drawableObjects;
		Dictionary<String, Texture2D> textures;

		public GraphicsEngine(Microsoft.Xna.Framework.Game game)
		{
			this.game = game;
			device = new GraphicsDeviceManager(game);
			drawableObjects = new List<GameObject>();
			textures = new Dictionary<String, Texture2D>();
		}

		public void LoadContent()
		{
			spriteBatch = new SpriteBatch(game.GraphicsDevice);

			// Load textures
			//Texture2D texture = game.Content.Load<Texture2D>("puck");
			textures.Add("puck", game.Content.Load<Texture2D>("puck"));
			textures.Add("box", game.Content.Load<Texture2D>("box"));
		}

		public void Draw(GameTime gameTime)
		{
			game.GraphicsDevice.Clear(Color.CornflowerBlue);

			// Draw all objects
			spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			foreach (GameObject obj in drawableObjects) {
				// Make sure texture is loaded
				if (!textures.ContainsKey(obj.spriteName)) {
					throw new System.ArgumentNullException("Attempted to draw sprite with an invalid texture.");
				}

				spriteBatch.Draw(textures[obj.spriteName], obj.getBoundingBox(), Color.White);
				//spriteBatch.Draw(textures[obj.spriteName], obj.position, Color.White);
				//device.GraphicsDevice.DrawUserPrimitives(PrimitiveType.PointList, new float[2] { obj.position.X, obj.position.Y }, 0, 1);
			}
			spriteBatch.End();
		}

		public void addObject(GameObject obj)
		{
			drawableObjects.Add(obj);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsDefense.GameState;
using Microsoft.Xna.Framework.Audio;

namespace PhysicsDefense.Graphics
{
	class GraphicsEngine
	{
		public GraphicsDeviceManager device;
		SpriteBatch spriteBatch;
		Game game;
        public SoundEffect explodeSound;
		
		List<GameObject> drawableObjects;
		Dictionary<String, Texture2D> textures;


		public GraphicsEngine(Microsoft.Xna.Framework.Game game)
		{
			this.game = game;
			device = new GraphicsDeviceManager(game);
			device.PreferredBackBufferWidth = 800;
			device.PreferredBackBufferHeight = 600;
			drawableObjects = new List<GameObject>();
			textures = new Dictionary<String, Texture2D>();
		}

		public void LoadContent()
		{
			spriteBatch = new SpriteBatch(game.GraphicsDevice);

			// Load textures
			textures.Add("puck", game.Content.Load<Texture2D>("puck"));
			textures.Add("box", game.Content.Load<Texture2D>("box"));
            for (int i = 1; i < 18; i++)
            {
                textures.Add("explode"+i.ToString(), game.Content.Load<Texture2D>("explode"+i.ToString()));
            }
            explodeSound = game.Content.Load<SoundEffect>("explode");

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

				//Rectangle dest = new Rectangle((int)obj.position.X, (int)obj.position.Y, (int)obj.size.X, (int)obj.size.Y);
				
			    Rectangle dest = new Rectangle((int)(obj.position.X * 10f), (int)(obj.position.Y * 10f), (int)(obj.size.X * 10f), (int)(obj.size.Y * 10f));
				Vector2 origin = new Vector2(textures[obj.spriteName].Width / 2, textures[obj.spriteName].Height / 2);
				spriteBatch.Draw(textures[obj.spriteName], dest, null, Color.White, obj.rotation, origin, SpriteEffects.None, 0);
			}
			spriteBatch.End();
		}

		public void addObject(GameObject obj)
		{
			drawableObjects.Add(obj);
		}

        public void removeObject(GameObject obj)
        {
            drawableObjects.Remove(obj);
        }
	}
}

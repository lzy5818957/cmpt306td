using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PhysicsDefense.GameState;

namespace PhysicsDefense.Graphics
{
	public class GraphicsEngine
	{
		public static int screenWidth = 1000;
		public static int screenHeight = 600;

		public GraphicsDeviceManager device;
		SpriteBatch spriteBatch;
		Game game;
        MessageBoard messageBoard;
		
		List<GameObject> drawableObjects;
		List<Texture2D> backgrounds;
		Dictionary<String, Texture2D> textures;


		public GraphicsEngine(Microsoft.Xna.Framework.Game game)
		{
            game.Content.RootDirectory = "Content";
			this.game = game;
			device = new GraphicsDeviceManager(game);
			device.PreferredBackBufferWidth = screenWidth;
			device.PreferredBackBufferHeight = screenHeight;
			device.PreferMultiSampling = true;
			device.ApplyChanges();

			drawableObjects = new List<GameObject>();
			backgrounds = new List<Texture2D>();
			textures = new Dictionary<String, Texture2D>();
            
		}

		public void LoadContent()
		{
			spriteBatch = new SpriteBatch(game.GraphicsDevice);
            SpriteFont msgFont = game.Content.Load<SpriteFont>("fonts/FirstFont");
            messageBoard = new MessageBoard(new Vector2(screenWidth, screenHeight),msgFont, "Game Starting...");
			// Load textures
            textures.Add("basicEnemy", game.Content.Load<Texture2D>("pictures/enemies/basicEnemy"));
            textures.Add("puck", game.Content.Load<Texture2D>("pictures/towers/puck"));
            textures.Add("basictower", game.Content.Load<Texture2D>("pictures/towers/basictower"));
            textures.Add("missiletower", game.Content.Load<Texture2D>("pictures/towers/missiletower"));
            textures.Add("missile",game.Content.Load<Texture2D>("pictures/bullets/missile"));
            textures.Add("connector", game.Content.Load<Texture2D>("pictures/towers/connector"));
            textures.Add("smoke", game.Content.Load<Texture2D>("pictures/bullets/smoke"));
			//textures.Add("basicTower", game.Content.Load<Texture2D>("basicTower"));

            for (int i = 1; i < 18; i++) {
                textures.Add("explode" + i.ToString(), game.Content.Load<Texture2D>("pictures/effect/explode/explode" + i.ToString()));
            }

            textures.Add("panel", game.Content.Load<Texture2D>("pictures/panel/panel"));
            textures.Add("sampleTower", game.Content.Load<Texture2D>("pictures/towers/basictower"));
		}

		public void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			game.GraphicsDevice.Clear(Color.CornflowerBlue);

			// Draw background
			foreach (Texture2D background in backgrounds) {
				spriteBatch.Draw(background, Vector2.Zero, Color.White);
			}

			// Draw all objects
			foreach (GameObject obj in drawableObjects) {
				// Make sure texture is loaded
				if (!textures.ContainsKey(obj.spriteName)) {
					throw new System.ArgumentNullException("Attempted to draw sprite with an invalid texture.");
				}

				Rectangle dest = new Rectangle(
					(int)(obj.position.X * GameWorld.worldScale),
					(int)(obj.position.Y * GameWorld.worldScale),
					(int)(obj.size.X * GameWorld.worldScale),
					(int)(obj.size.Y * GameWorld.worldScale)
				);
				Vector2 origin = new Vector2(textures[obj.spriteName].Width / 2, textures[obj.spriteName].Height / 2);

				spriteBatch.Draw(textures[obj.spriteName], dest, null, obj.color, obj.rotation, origin, SpriteEffects.None, 0);
			}

            messageBoard.draw(spriteBatch);
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

		public void addBackground(Texture2D texture)
		{
			backgrounds.Add(texture);
		}

        public void addBackground(String file)
        {
            backgrounds.Add(game.Content.Load<Texture2D>(file));
        }
	}
}

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
		public Dictionary<String, Texture2D> textures;


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
            textures.Add("basicbullet", game.Content.Load<Texture2D>("pictures/bullets/basicbullet"));
            textures.Add("basictower", game.Content.Load<Texture2D>("pictures/towers/basictower"));
            textures.Add("missiletower", game.Content.Load<Texture2D>("pictures/towers/missiletower"));
            textures.Add("herotower", game.Content.Load<Texture2D>("pictures/towers/herotower"));
            textures.Add("missile",game.Content.Load<Texture2D>("pictures/bullets/missile"));
            textures.Add("aoemissile", game.Content.Load<Texture2D>("pictures/bullets/aoemissile"));
            textures.Add("connector", game.Content.Load<Texture2D>("pictures/towers/connector"));
            textures.Add("smoke", game.Content.Load<Texture2D>("pictures/bullets/smoke"));
            textures.Add("aoesmoke", game.Content.Load<Texture2D>("pictures/bullets/aoesmoke"));
			textures.Add("spinner", game.Content.Load<Texture2D>("pictures/effect/spinner"));
            textures.Add("gold", game.Content.Load<Texture2D>("pictures/panel/gold"));
            textures.Add("life", game.Content.Load<Texture2D>("pictures/panel/life"));
            textures.Add("left", game.Content.Load<Texture2D>("pictures/towermenu/left"));
            textures.Add("right", game.Content.Load<Texture2D>("pictures/towermenu/right"));
            textures.Add("sell", game.Content.Load<Texture2D>("pictures/towermenu/sell"));
            for (int i = 1; i < 18; i++) {
                textures.Add("explode" + i.ToString(), game.Content.Load<Texture2D>("pictures/effect/explode/explode" + i.ToString()));
            }

            textures.Add("panel", game.Content.Load<Texture2D>("pictures/panel/panel"));
            textures.Add("sampleBTower", game.Content.Load<Texture2D>("pictures/towers/basictower"));
            textures.Add("sampleMTower", game.Content.Load<Texture2D>("pictures/towers/missiletower"));
            textures.Add("sampleHTower", game.Content.Load<Texture2D>("pictures/towers/herotower"));

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

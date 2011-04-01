using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class MessageBoard
    {
        SpriteFont messageFont;
        static String message;
        Vector2 screenSize;
        static String[] menuOption;
        public MessageBoard(Vector2 sSize,SpriteFont msgFont,String msg)
        {
            message = msg;
            screenSize=sSize;
            messageFont = msgFont;
        }

        public static void updateMessage(String msg)
        {
            message = msg;
        }
        public static void updateMenu(Tower selectedTower)
        {
            if (selectedTower == null) return;
            if (typeof(BasicTower) == selectedTower.GetType())
            {
                menuOption = new string[] { "life", "life", "life", "life", "life" }; 
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                messageFont,
                message,
                new Vector2(
                    (screenSize.X) - 130,20), Color.White);

            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["gold"], new Vector2((screenSize.X) - 170, 15), null, Color.White);
            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["life"], new Vector2((screenSize.X) - 170, 65), null, Color.White);
            if (menuOption != null)
            {
                foreach (String texture in menuOption)
                {
                    spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures[texture], new Vector2((screenSize.X) - 170, 300), null, Color.White);
                }
            }
        }
    }
}

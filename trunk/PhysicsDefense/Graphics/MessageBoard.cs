using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace PhysicsDefense.GameState
{
    class MessageBoard
    {
        SpriteFont messageFont;
        static String message;
        Vector2 screenSize;
        static String[] menuOption;
        static Tower currentTower;
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
            currentTower = selectedTower;
            if (currentTower == null) return;
            if (typeof(BasicTower) == selectedTower.GetType())
            {
                menuOption = new string[] { "sell" }; 
            }
            else if (typeof(MissileTower) == selectedTower.GetType())
            {
                menuOption = new string[] { "sell" }; 
            }
            else if (typeof(HeroTower) == selectedTower.GetType())
            {
                menuOption = new string[] { "sell","left" ,"right"};
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                messageFont,
                message,
                new Vector2(
                    (screenSize.X) - 130,30), Color.Black);

            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["gold"], new Vector2((screenSize.X) - 170, 35), null, Color.White);
            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["life"], new Vector2((screenSize.X) - 170, 85), null, Color.White);

            if (menuOption != null)
            {
                int i = 0;
                foreach (String texture in menuOption)
                {
                    i++;
                    spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures[texture], new Vector2((screenSize.X) - 245 + i*55, 380), null, Color.White);
                }
                i = 0;
            }
            if (currentTower != null)
            {
                if (typeof(BasicTower) == currentTower.GetType())
                {
                    Console.WriteLine(GameWorld.mouseState.X + ":" + GameWorld.mouseState.Y);

                    if(GameWorld.mouseState.X > 816 
                        && GameWorld.mouseState.X < 860 
                        && GameWorld.mouseState.Y >382 
                        && GameWorld.mouseState.Y < 426 
                        && GameWorld.mouseState.LeftButton  == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell()/2;
                        currentTower = null;
                    }
                }
                else if (typeof(MissileTower) == currentTower.GetType())
                {
                    Console.WriteLine(GameWorld.mouseState.X + ":" + GameWorld.mouseState.Y);

                    if (GameWorld.mouseState.X > 816
                        && GameWorld.mouseState.X < 860
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell() / 2;
                        currentTower = null;
                    }
                }
                else if (typeof(HeroTower) == currentTower.GetType())
                {
                    Console.WriteLine(GameWorld.mouseState.X + ":" + GameWorld.mouseState.Y);

                    if (GameWorld.mouseState.X > 816
                        && GameWorld.mouseState.X < 860
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell() / 2;
                        currentTower = null;
                    }

                    if (GameWorld.mouseState.X > 866
                        && GameWorld.mouseState.X < 904
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed
                        ){
                            ((HeroTower)currentTower).upgradeRange();    
                            currentTower = null;
                    }
                    if (GameWorld.mouseState.X > 910
                        && GameWorld.mouseState.X < 954
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed
                        )
                    {
                        ((HeroTower)currentTower).upgradeSpeed();
                        currentTower = null;
                    }
                }
            }
        }
    }
}

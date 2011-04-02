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
        SpriteFont infoFont;
        static String message;
        Vector2 screenSize;
        static String[] menuOption;
        static String talent="";
        static Tower currentTower;
        public MessageBoard(Vector2 sSize,SpriteFont msgFont,SpriteFont iFont,String msg)
        {
            message = msg;
            screenSize=sSize;
            messageFont = msgFont;
            infoFont=iFont;
        }

        public static void updateMessage(String msg)
        {
            message = msg;
        }
        public static void updateMenu(Tower selectedTower)
        {
            currentTower = selectedTower;
        }

        private void update()
        {
            if (currentTower == null)
            {
                menuOption = new string[] { };
                talent = "";
                return;
            }
            if (typeof(BasicTower) == currentTower.GetType())
            {
                menuOption = new string[] { "sell" };
                talent = "Basic Tower";
            }
            else if (typeof(MissileTower) == currentTower.GetType())
            {
                menuOption = new string[] { "sell" };
                talent = "AoE Missile Tower";
            }
            else if (typeof(HeroTower) == currentTower.GetType())
            {
                menuOption = new string[] { "sell", "range", "speed" };

                talent = "Point(s):" + ((HeroTower)currentTower).getAvailablePoint() + "\nLevel(s):" + ((HeroTower)currentTower).getLevel();
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            update();
            spriteBatch.DrawString(
                messageFont,
                message,
                new Vector2(
                    (screenSize.X) - 130,30), Color.Black);

            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["gold"], new Vector2((screenSize.X) - 170, 35), null, Color.White);
            spriteBatch.Draw(ResourceManager.getGraphicsEngine().textures["life"], new Vector2((screenSize.X) - 170, 85), null, Color.White);
            spriteBatch.DrawString(infoFont, talent, new Vector2((screenSize.X - 190), 440), Color.Black);

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
            if (currentTower != null && currentTower.isActivated == true)
            {
                if (typeof(BasicTower) == currentTower.GetType())
                {


                    if(GameWorld.mouseState.X > 816 
                        && GameWorld.mouseState.X < 860 
                        && GameWorld.mouseState.Y >382 
                        && GameWorld.mouseState.Y < 426 
                        && GameWorld.mouseState.LeftButton  == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell()/2;
                        updateMenu(null);
                    }
                }
                else if (typeof(MissileTower) == currentTower.GetType())
                {


                    if (GameWorld.mouseState.X > 816
                        && GameWorld.mouseState.X < 860
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell() / 2;
                        updateMenu(null);
                    }
                }
                else if (typeof(HeroTower) == currentTower.GetType())
                {


                    if (GameWorld.mouseState.X > 816
                        && GameWorld.mouseState.X < 860
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseState.LeftButton == ButtonState.Pressed)
                    {
                        GameWorld.money += currentTower.sell() / 2;
                        updateMenu(null);
                    }

                    if (GameWorld.mouseState.X > 871
                        && GameWorld.mouseState.X < 915
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseLeftPress == true
                        )
                    {
                            ((HeroTower)currentTower).upgradeRange();
                    }
                    if (GameWorld.mouseState.X > 926
                        && GameWorld.mouseState.X < 970
                        && GameWorld.mouseState.Y > 382
                        && GameWorld.mouseState.Y < 426
                        && GameWorld.mouseLeftPress == true
                        )
                    {
                        ((HeroTower)currentTower).upgradeSpeed();
                    }
                }
            }
        }
    }
}

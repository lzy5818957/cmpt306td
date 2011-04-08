using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.Graphics
{
    class InfoBoard
    {
        public static String information;
        public static Color infoColor=Color.White;
        private Vector2 position;
        private SpriteFont infoFont;
        //use frameCount to determine how long the message displayed
        private static int frameCount=100;
        public InfoBoard(SpriteFont iFont,Vector2 pos,String info,int time)
        {
            infoFont = iFont;
            position = pos;
            information = info;
            frameCount=time;
        }

        public static void updateInfo(String info, Color iColor,int time)
        {
            information = info;
            infoColor = iColor;
            frameCount = time;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (frameCount >= 0)
            {
                spriteBatch.DrawString(
                    infoFont,
                    information,position, infoColor);
                frameCount--;
            }
        }
    }
}

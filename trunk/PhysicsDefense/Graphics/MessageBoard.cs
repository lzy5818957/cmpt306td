﻿using System;
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

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                messageFont,
                message,
                new Vector2(
                    (screenSize.X / 2) - 50,
                    (screenSize.Y) - 100
                    ),
                Color.White);
        }
    }
}
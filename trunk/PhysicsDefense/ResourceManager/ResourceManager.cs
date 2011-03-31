using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Graphics;
using PhysicsDefense.Audio;
using PhysicsDefense.GameState;

namespace PhysicsDefense
{
    class ResourceManager
    {
        private static GraphicsEngine graphics;
        private static GameAudio audio;
        private static GameWorld gameWorld;
        public static Game game;


        public static GraphicsEngine getGraphicsEngine()
        {
            if (game == null)
            {
                throw new NullReferenceException("Make sure you set game for this class before you get any resource");
            }
            if (graphics == null)
            {
                graphics = new GraphicsEngine(game);
                
            }
            return graphics;
        }

        public static GameAudio getGameAudio()
        {
            if (game == null)
            {
                throw new NullReferenceException("Make sure you set game for this class before you get any resource");
            }
            if (audio == null)
            {
                audio = new GameAudio(game);

            }
            return audio;
        }

        public static GameWorld getGameWorld()
        {
            if (game == null)
            {
                throw new NullReferenceException("Make sure you set game for this class before you get any resource");
            }
            if (gameWorld == null)
            {
                gameWorld = new GameWorld((PhysicsDefense)game);

            }
            return gameWorld;
        }

    }


}

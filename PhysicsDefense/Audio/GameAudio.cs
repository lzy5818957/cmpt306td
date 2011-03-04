using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace PhysicsDefense.Audio
{
	public class GameAudio
	{
		Microsoft.Xna.Framework.Game game;
		Dictionary<String, SoundEffect> sounds;

		public GameAudio(Microsoft.Xna.Framework.Game game)
		{
			this.game = game;
			sounds = new Dictionary<String, SoundEffect>();
		}

		public void LoadContent()
		{
			sounds.Add("explode", game.Content.Load<SoundEffect>("explode"));
		}

		public void PlaySound(String soundName)
		{
			sounds[soundName].Play();
		}
	}
}

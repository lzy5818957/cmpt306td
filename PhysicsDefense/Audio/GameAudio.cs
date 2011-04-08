using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PhysicsDefense.Audio
{
	public class GameAudio
	{
		Microsoft.Xna.Framework.Game game;
		Dictionary<String, SoundEffect> sounds;
		Song music;

		public GameAudio(Microsoft.Xna.Framework.Game game)
		{
			this.game = game;
			sounds = new Dictionary<String, SoundEffect>();
		}

		public void LoadContent()
		{
			music = game.Content.Load<Song>("audio/music");
            sounds.Add("explode", game.Content.Load<SoundEffect>("audio/explode"));
            sounds.Add("explodebystuck", game.Content.Load<SoundEffect>("audio/explodebystuck"));
            sounds.Add("missilelaunch", game.Content.Load<SoundEffect>("audio/missilelaunch"));
            sounds.Add("missilehit", game.Content.Load<SoundEffect>("audio/missilehit"));
            sounds.Add("bullethit", game.Content.Load<SoundEffect>("audio/bullethit"));
            sounds.Add("cannonshot", game.Content.Load<SoundEffect>("audio/cannonshot"));
            sounds.Add("upgrade", game.Content.Load<SoundEffect>("audio/upgrade"));
            sounds.Add("click", game.Content.Load<SoundEffect>("audio/click"));
            sounds.Add("wavestart", game.Content.Load<SoundEffect>("audio/wavestart"));
            sounds.Add("error", game.Content.Load<SoundEffect>("audio/error"));
            sounds.Add("spin", game.Content.Load<SoundEffect>("audio/spin"));
            sounds.Add("heromissile", game.Content.Load<SoundEffect>("audio/heromissile"));
		}

		public void PlaySound(String soundName)
		{
			sounds[soundName].Play();
		}

		public void PlayMusic()
		{
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(music);
		}

        public void clearList()
        {
            sounds.Clear();
        }
	}
}

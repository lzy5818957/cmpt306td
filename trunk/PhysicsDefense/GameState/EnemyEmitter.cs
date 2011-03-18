using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState {
	public delegate void SpawnHandler(EnemyType enemy);
	public delegate void WaveHandler();

	class EnemyEmitter
	{
		public int wave { get; private set; }
		private int waveEnemiesSpawned = 0;
		private double timer = 0;
		public bool active = false;
		public bool waveFinished = false;

		public Vector2 position { get; private set; }

		public SpawnHandler onSpawn;
		public WaveHandler onWaveFinished;

		public EnemyEmitter(Vector2 position, int wave)
		{
			this.wave = wave;
			this.position = position;
		}

		public void nextWave()
		{
			timer = 0;
			wave++;
			waveEnemiesSpawned = 0;
		}

		public void start()
		{
			active = true;
			waveFinished = false;
		}

		public void update(GameTime gameTime)
		{
			if (!active)
				return;

			// Spawn new enemy every second
			timer += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (timer > 1000) {
				onSpawn(EnemyType.Normal);
				waveEnemiesSpawned++;
				timer -= 1000;
			}

			// Check if this was the end of the wave
			if (waveEnemiesSpawned >= wave + 9) {
				active = false;
				waveFinished = true;
				//onWaveFinished();
			}
		}
	}
}

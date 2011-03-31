using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PhysicsDefense.Physics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PhysicsDefense.GameState
{
	public enum EnemyType
	{
		Normal
	}

	public class Marble : GameObject
	{
		private float radius = 0.25f;

		private float baseHealth = 100;
		private float baseBounty = 5;
		public float health;

		private float stuckFactor;
		private const float stuckLimit = 30f;
		private const float stuckThreshholdSpeed = 0.5f;

		// The amount of money awarded when the marble is killed
		public float bounty;

		public Marble(World world, Vector2 position, float healthMult, float bountyMult)
		{
			this.world = world;
			spriteName = "basicEnemy";
			physicsProperties.body = BodyFactory.CreateCircle(world, radius, 3.0f, position);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Dynamic;
			physicsProperties.body.Friction = 0.8f;
			physicsProperties.body.AngularDamping = 0f;
			physicsProperties.body.CollisionCategories = Category.Cat1;
            physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat3 | Category.Cat5;
            physicsProperties.body.UserData = this;

			health = baseHealth * healthMult;
			bounty = baseBounty * bountyMult;
		}

		public override void update(GameTime gameTime)
        {
			// Handle anti-stuck
			if (physicsProperties.speed < stuckThreshholdSpeed) {
				stuckFactor += stuckThreshholdSpeed - physicsProperties.speed;
			}

			if (stuckFactor > stuckLimit) {
				// Marble was stuck for too long
				die();
			}

			color.G = color.B = (byte)(255f - (255f * (stuckFactor / stuckLimit)));

			stuckFactor -= stuckThreshholdSpeed / 2f;
			if (stuckFactor < 0f) {
				stuckFactor = 0f;
				color = nativeColor;
			}

			base.update(gameTime);
        }

		public void takeDamage(int damage)
		{
			health -= damage;
			if (health <= 0)
				die();
		}

		public override void die()
		{
			Explode explosion = new Explode(world, position);
			onCreateObject(explosion);
			base.die();
		}
	}
}

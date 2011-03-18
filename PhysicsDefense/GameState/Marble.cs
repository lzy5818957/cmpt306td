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
		private float baseHealth = 100;
		public float health;

		public Marble(World world, Vector2 position, float healthMult)
		{
			this.world = world;
			spriteName = "basicEnemy";
			physicsProperties.body = BodyFactory.CreateCircle(world, 0.25f, 3.0f, position);
			physicsProperties.body.Restitution = 0.2f;
			physicsProperties.body.BodyType = BodyType.Dynamic;
			physicsProperties.body.Friction = 0.8f;
			physicsProperties.body.AngularDamping = 0f;
			physicsProperties.body.CollisionCategories = Category.Cat1;
            physicsProperties.body.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat3 | Category.Cat5;
            physicsProperties.body.UserData = this;

			health = baseHealth * healthMult;
		}

		public override void update(GameTime gameTime)
        {
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

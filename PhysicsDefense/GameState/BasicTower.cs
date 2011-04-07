using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class BasicTower:Tower
    {
        public BasicTower(World world,Vector2 position):base(world, position)
        {
            // Game-related properties
            rechargeTime = 600;
            range = 2f;
            cost = 50f;
            spriteName = "basictower";
        }

        public override void shoot()
        {
            base.shoot();
            if (enemiesInRange.Count <= 0)
                return;
            //Bullet Creatiion
            Marble target = enemiesInRange[0];
            
            //Calculate the direction of the bullet according to the position and the velocity of the Marble
            double a = Math.Pow(target.physicsProperties.velocity.X, 2) + Math.Pow(target.physicsProperties.velocity.Y, 2) - Math.Pow(Bullet.speed, 2);
            double b = 2 * ((target.position.X - position.X) * target.physicsProperties.velocity.X + (target.position.Y - position.Y) * target.physicsProperties.velocity.Y);
            double c = Math.Pow(target.position.X - position.X, 2) + Math.Pow(target.position.X - position.X, 2);
            double t = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            if(t<0)
                t = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            float dirX = (target.position.X + (float)t * target.physicsProperties.velocity.X - position.X) / ((float)t * Bullet.speed);
            float dirY = (target.position.Y + (float)t * target.physicsProperties.velocity.Y - position.Y) / ((float)t * Bullet.speed);
            Vector2 direction = new Vector2(dirX, dirY);
            
            //Vector2 direction = new Vector2((target.position.X - position.X), (target.position.Y - position.Y));
            
            Bullet newBullet = new Bullet(world, position, direction);
            onCreateObject(newBullet);
        }
    }
}

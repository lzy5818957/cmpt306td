﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class BaseTower:Tower
    {
        public BaseTower(World world,Vector2 position):base(world, position)
        {
            // Game-related properties
            rechargeTime = 500;
            range = 2f;
            cost = 30f;
            spriteName = "tower";
        }

        public override void shoot()
        {
            base.shoot();
            if (enemiesInRange.Count <= 0)
                return;
            //Bullet Creatiion
            Marble target = enemiesInRange[0];
            Vector2 direction = new Vector2((target.position.X - position.X), (target.position.Y - position.Y));
            Bullet newBullet = new Bullet(world, position, direction);
            onCreateObject(newBullet);
        }
    }
}

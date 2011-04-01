using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace PhysicsDefense.GameState
{
    class MissileTower:Tower
    {
        public MissileTower(World world,Vector2 position):base(world, position)
        {
            // Game-related properties
            rechargeTime = 1000;
            range = 2f;
            cost = 100f;
            spriteName = "missiletower";
        }

        public override void shoot()
        {
            base.shoot();
            if (enemiesInRange.Count <= 0)
                return;
            //Missile Creation
            Marble target = enemiesInRange[0];
            AoeMissile newMissile = new AoeMissile(world, position, target);
            onCreateObject(newMissile);
        }
    }
}

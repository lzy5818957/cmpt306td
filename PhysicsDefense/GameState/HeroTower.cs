using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class HeroTower:Tower
    {
        static List<HeroTower> heroTowers=new List<HeroTower>();
        float experience;
        int level = 1;
        int availablePoint = 1;
        public HeroTower(World world,Vector2 position):base(world, position)
        {
            // Game-related properties
            rechargeTime = 1000;
            range = 2f;
            cost = 100f;
            spriteName = "herotower";
            experience = 0;
            heroTowers.Add(this);
        }

        public override void shoot()
        {
            if (Math.Log(experience/WaveData.initialEnemyCount,2+WaveData.bountyMult)>level)
            {
                level++;
                availablePoint++;
                rechargeTime -= 10;
                range +=0.01f;
                Console.WriteLine("Tower upgraded to level "+level);
            }
            base.shoot();
            if (enemiesInRange.Count <= 0)
                return;
            //Bullet Creatiion
            foreach (Marble target in enemiesInRange)
            {
                Missile newMissile = new Missile(world, position, target);
                onCreateObject(newMissile);
            }
        }

        internal static void gainExperiences(float bounty)
        {
            int i = heroTowers.Count;
            if (i <= 0)
                return;
            foreach (HeroTower heroTower in heroTowers)
            {
                heroTower.experience += (bounty / i);
            }
        }

        public bool upgradeRange()
        {
            if (availablePoint <= 0)
                return false;
            availablePoint--;
            range += 0.1f;
            return true;
        }

        public bool upgradeSpeed()
        {
            if (availablePoint <= 0)
                return false;
            availablePoint--;
            rechargeTime-= 10;
            return true;
        }
    }
}

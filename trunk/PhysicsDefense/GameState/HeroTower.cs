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
            if (experience > 5 && level <= 1)
            {
                level++;
                rechargeTime = 500;
                range =4f;
                Console.WriteLine("Tower upgraded.");
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
    }
}

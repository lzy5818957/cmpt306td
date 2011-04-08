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
            range = 1.6f;
            cost = 300f;
            spriteName = "herotower";
            experience = 0;
            heroTowers.Add(this);
        }

        public override void shoot()
        {
            base.shoot();
            if (enemiesInRange.Count <= 0)
                return;
            //Bullet Creatiion
            foreach (Marble target in enemiesInRange)
            {
                Missile newMissile = new Missile(world, position, target);
                onCreateObject(newMissile);
            }
            onPlaySound("heromissile");
        }

        internal static void gainExperiences(float bounty)
        {
            int i = heroTowers.Count;
            if (i <= 0)
                return;
            foreach (HeroTower heroTower in heroTowers)
            {
                heroTower.experience += (float)(bounty / Math.Pow(i, 0.33));
            }
        }

        public void upgradeRange()
        {
            isSelected = true;
            if (availablePoint <= 0)
            {
                return;
            }
            availablePoint--;
            //range += 0.15f;
            range = (float)Math.Sqrt(Math.Pow(0.9, 2) + Math.Pow(range, 2));
            onPlaySound("upgrade");
           
        }

        public void upgradeSpeed()
        {
            isSelected = true;
            if (availablePoint <= 0)
                return;
            availablePoint--;
            rechargeTime *= 0.92f;
            onPlaySound("upgrade");
        }

        public override void die()
        {
            heroTowers.Remove(this);
            base.die();
        }

        public int getLevel()
        {
            return level;
        }

        public int getAvailablePoint()
        {
            return availablePoint;
        }

        public override void update(GameTime gameTime)
        {
            //if (Math.Log(experience/WaveData.initialEnemyCount,2+WaveData.bountyMult)>level)
            //float expCheck = (float)(Math.Log(experience + 1) * Math.Log(experience + 1) * 0.05f);
            float expCheck = (float)(Math.Pow(experience, 0.88) * 0.025f);
            if (expCheck > level)
            {
                level++;
                availablePoint++;
                //rechargeTime *= 0.9f;
                //range +=0.015f;
            }
            base.update(gameTime);
        }
    }
}

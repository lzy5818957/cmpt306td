using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class SampleBTower : SampleTower
    {

        public SampleBTower(World world, Vector2 position) : base(world, position) {
            spriteName = "sampleBTower";
        }
    }
}

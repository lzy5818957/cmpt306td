using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class SampleHTower : SampleTower
    {
        public SampleHTower(World world, Vector2 position) : base(world, position) {

            spriteName = "sampleHTower";
        }
    }
}

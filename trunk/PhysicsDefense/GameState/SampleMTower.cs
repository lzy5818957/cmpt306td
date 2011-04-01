﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace PhysicsDefense.GameState
{
    class SampleMTower : SampleTower
    {
        public SampleMTower(World world, Vector2 position):base(world, position)
        {
            spriteName = "sampleMTower";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [Serializable]
    public class LandUnit: BattleFieldElement
    {
        public LandUnitStats stats;

        public override string ToString()
        {
            return "LandUnit(" + positionProps + ", stats: " + stats.ToString() + ")";
        }
    }
}

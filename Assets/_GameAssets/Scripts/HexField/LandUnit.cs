using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [Serializable]
    public class LandUnit: BattleFieldElement, Destructable
    {
        public LandUnitStats stats;

        public Tuple<int, int> hpRange => Tuple.Create(stats.hp, stats.maxHP);

        public override string ToString()
        {
            return "LandUnit(" + positionProps + ", stats: " + stats.ToString() + ")";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class LandObstacle: BattleFieldElement, Destructable
    {
        public int hp = 1;
        public int maxHP = 1;

        public Tuple<int, int> hpRange => Tuple.Create(hp, maxHP);

        public override string ToString()
        {
            return "LandObstacle(" + positionProps + ", hp: " + hp.ToString() + ")";
        }
    }
}

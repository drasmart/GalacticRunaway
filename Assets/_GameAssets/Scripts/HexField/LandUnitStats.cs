using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class LandUnitStats
    {
        public int hp = 1;
        public int maxHP = 1;

        public int armor = 0;
        public int damage = 1;

        public int actionPoints = 0;
        public int maxActionPoints = 1;

        public override string ToString()
        {
            return "LandUnitStats(hp: " + hp.ToString() + "/" + maxHP.ToString() + ")";
        }
    }
}

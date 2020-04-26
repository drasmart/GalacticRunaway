using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public struct LandUnitStats
    {
        public int hp;
        public int maxHP;

        public int armor;
        public int damage;

        public int actionPoints;
        public int maxActionPoints;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class LandObstacle: BattleFieldElement
    {
        public int hp = 1;

        public override string ToString()
        {
            return "LandObstacle(" + positionProps + ", hp: " + hp.ToString() + ")";
        }
    }
}

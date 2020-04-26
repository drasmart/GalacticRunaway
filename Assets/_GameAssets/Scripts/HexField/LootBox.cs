using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class LootBox: BattleFieldElement
    {
        public int points = 1;

        public override string ToString()
        {
            return "LootBox(" + positionProps + ", points: " + points.ToString() + ")";
        }
    }
}

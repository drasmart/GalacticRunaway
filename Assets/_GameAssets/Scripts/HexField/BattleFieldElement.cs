using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexGrid;

namespace HexField
{
    [System.Serializable]
    public abstract class BattleFieldElement
    {
        public HexCoords2Int coords;
        public float rotation;

        public string positionProps => "coords: " + coords.ToString() + ", rotation: " + rotation.ToString();
    }
}

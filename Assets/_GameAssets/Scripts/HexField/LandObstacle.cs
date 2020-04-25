using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class LandObstacle
    {
        public HexCoords2Int coords;
        public int hp;
    }
}

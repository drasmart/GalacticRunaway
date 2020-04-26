using HexGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [Serializable]
    public class LandUnit
    {
        public HexCoords2Int coords;
        public float rotation;
        public LandUnitStats stats;
    }
}

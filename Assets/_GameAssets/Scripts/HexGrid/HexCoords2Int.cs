using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    [System.Serializable]
    public struct HexCoords2Int
    {
        public static readonly float yStep = HexCoords3.yStep;

        public int u;
        public int v;

        public HexCoords2Int(int u, int v)
        {
            this.u = u;
            this.v = v;
        }

        public static implicit operator HexCoords2(HexCoords2Int coords)
        {
            return new HexCoords2(coords.u, coords.v);
        }
        public static implicit operator HexCoords2Int(HexCoords2 coords)
        {
            return new HexCoords2Int(Mathf.RoundToInt(coords.u), Mathf.RoundToInt(coords.v));
        }

        public static implicit operator HexCoords3Int(HexCoords2Int coords)
        {
            return new HexCoords3Int(coords.u, coords.v, 0);
        }
        public static implicit operator HexCoords2Int(HexCoords3Int coords)
        {
            return new HexCoords2Int(coords.u, coords.v);
        }

        public string toString()
        {
            return "HexCoords2Int(" + u + ", " + v + ")";
        }
    }
}

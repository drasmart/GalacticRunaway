using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    [System.Serializable]
    public struct HexCoords3Int
    {
        public static readonly float yStep = Mathf.Sqrt(3) / 2;

        public int u;
        public int v;
        public int y;

        public HexCoords3Int(int u, int v, int y)
        {
            this.u = u;
            this.v = v;
            this.y = y;
        }

        public static implicit operator HexCoords3(HexCoords3Int coords)
        {
            return new HexCoords3(coords.u, coords.v, coords.y);
        }
        public static implicit operator HexCoords3Int(HexCoords3 coords)
        {
            return new HexCoords3Int(Mathf.RoundToInt(coords.u), Mathf.RoundToInt(coords.v), Mathf.RoundToInt(coords.y));
        }

        public string toString()
        {
            return "HexCoords2Int(" + u + ", " + v + ")";
        }
    }
}

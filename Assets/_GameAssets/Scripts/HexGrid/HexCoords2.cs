using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    [System.Serializable]
    public struct HexCoords2
    {
        public static Vector2 q;
        public static readonly float yStep = HexCoords3.yStep;

        public float u;
        public float v;

        public HexCoords2(float u, float v)
        {
            this.u = u;
            this.v = v;
        }

        public static implicit operator HexCoords3(HexCoords2 coords)
        {
            return new HexCoords3(coords.u, coords.v, 0);
        }
        public static implicit operator HexCoords2(HexCoords3 coords)
        {
            return new HexCoords2(coords.u, coords.v);
        }

        public override string ToString()
        {
            return "HexCoords2(" + u + ", " + v + ")";
        }
        public Vector3 ToVector3()
        {
            return ((HexCoords3)this).ToVector3();
        }
    }
}

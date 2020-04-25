using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    [System.Serializable]
    public struct HexCoords3
    {
        public static readonly float yStep = Mathf.Sqrt(3) / 2;

        public float u;
        public float v;
        public float y;

        public Vector3 ToVector3()
        {
            float x = u - v / 2;
            float z = v * yStep;
            return new Vector3(x, y, z);
        }
        public HexCoords3(float u, float v, float y)
        {
            this.u = u;
            this.v = v;
            this.y = y;
        }
        public HexCoords3(Vector3 position)
        {
            v = position.z / yStep;
            u = position.x + v / 2;
            y = position.y;
        }

        public override string ToString()
        {
            return "HexCoords3(" + u + ", " + v + ", " + y + ")";
        }
    }
}

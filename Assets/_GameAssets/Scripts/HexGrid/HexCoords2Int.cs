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

        public override string ToString()
        {
            return "HexCoords2Int(" + u + ", " + v + ")";
        }
        public Vector3 ToVector3()
        {
            return ((HexCoords2)this).ToVector3();
        }


        public static HexCoords2Int operator +(HexCoords2Int lhs, HexCoords2Int rhs)
        {
            return new HexCoords2Int(lhs.u + rhs.u, lhs.v + rhs.v);
        }
        public static HexCoords2Int operator -(HexCoords2Int lhs, HexCoords2Int rhs)
        {
            return lhs + (-rhs);
        }
        public static HexCoords2Int operator -(HexCoords2Int thiz)
        {
            return new HexCoords2Int(-thiz.u, -thiz.v);
        }
        public static HexCoords2Int operator *(HexCoords2Int thiz, int k)
        {
            return new HexCoords2Int(thiz.u * k, thiz.v * k);
        }
        public static HexCoords2Int operator *(int k, HexCoords2Int thiz)
        {
            return thiz * k;
        }

        public static bool operator ==(HexCoords2Int lhs, HexCoords2Int rhs)
        {
            return (lhs.u == rhs.u) && (lhs.v == rhs.v);
        }
        public static bool operator !=(HexCoords2Int lhs, HexCoords2Int rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return u ^ v;
        }

        public static readonly HexCoords2Int zero = new HexCoords2Int(0, 0);
        public static readonly HexCoords2Int one = new HexCoords2Int(1, 1);
        public static readonly HexCoords2Int uPlus  = new HexCoords2Int(1, 0);
        public static readonly HexCoords2Int vPlus  = new HexCoords2Int(0, 1);
        public static readonly HexCoords2Int uMinus = new HexCoords2Int(-1, 0);
        public static readonly HexCoords2Int vMinus = new HexCoords2Int(0, -1);
    }
}

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

        public override string ToString()
        {
            return "HexCoords2Int(" + u + ", " + v + ")";
        }
        public Vector3 ToVector3()
        {
            return ((HexCoords3)this).ToVector3();
        }

        public static HexCoords3Int operator +(HexCoords3Int lhs, HexCoords3Int rhs)
        {
            return new HexCoords3Int(lhs.u + rhs.u, lhs.v + rhs.v, lhs.y + rhs.y);
        }
        public static HexCoords3Int operator -(HexCoords3Int lhs, HexCoords3Int rhs)
        {
            return lhs + (-rhs);
        }
        public static HexCoords3Int operator -(HexCoords3Int thiz)
        {
            return new HexCoords3Int(-thiz.u, -thiz.v, -thiz.y);
        }
        public static HexCoords3Int operator *(HexCoords3Int thiz, int k)
        {
            return new HexCoords3Int(thiz.u * k, thiz.v * k, thiz.y * k);
        }
        public static HexCoords3Int operator *(int k, HexCoords3Int thiz)
        {
            return thiz * k;
        }

        public static bool operator ==(HexCoords3Int lhs, HexCoords3Int rhs)
        {
            return (lhs.u == rhs.u) && (lhs.v == rhs.v) && (lhs.y == rhs.y);
        }
        public static bool operator !=(HexCoords3Int lhs, HexCoords3Int rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return u ^ v ^ y;
        }
        public override bool Equals(object obj)
        {
            if (obj is HexCoords3Int)
            {
                var other = (HexCoords3Int)obj;
                return this == other;
            }
            return false;
        }

        public static readonly HexCoords3Int zero = new HexCoords3Int(0, 0, 0);
        public static readonly HexCoords3Int one = new HexCoords3Int(1, 1, 1);
        public static readonly HexCoords3Int uPlus = new HexCoords3Int(1, 0, 0);
        public static readonly HexCoords3Int vPlus = new HexCoords3Int(0, 1, 0);
        public static readonly HexCoords3Int yPlus = new HexCoords3Int(0, 0, 1);
        public static readonly HexCoords3Int uMinus = new HexCoords3Int(-1, 0, 0);
        public static readonly HexCoords3Int vMinus = new HexCoords3Int(0, -1, 0);
        public static readonly HexCoords3Int yMinus = new HexCoords3Int(0, 0, -1);
    }
}

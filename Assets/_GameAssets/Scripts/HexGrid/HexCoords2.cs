using System;
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

        public static HexCoords2 operator +(HexCoords2 lhs, HexCoords2 rhs)
        {
            return new HexCoords2(lhs.u + rhs.u, lhs.v + rhs.v);
        }
        public static HexCoords2 operator -(HexCoords2 lhs, HexCoords2 rhs)
        {
            return lhs + (-rhs);
        }
        public static HexCoords2 operator -(HexCoords2 thiz)
        {
            return new HexCoords2(-thiz.u, -thiz.v);
        }
        public static HexCoords2 operator *(HexCoords2 thiz, int k)
        {
            return new HexCoords2(thiz.u * k, thiz.v * k);
        }
        public static HexCoords2 operator *(int k, HexCoords2 thiz)
        {
            return thiz * k;
        }
        public static HexCoords2 operator /(HexCoords2 thiz, int k)
        {
            return thiz * (1 / k);
        }
        public static HexCoords2 operator *(HexCoords2 thiz, float k)
        {
            return new HexCoords2(thiz.u * k, thiz.v * k);
        }
        public static HexCoords2 operator *(float k, HexCoords2 thiz)
        {
            return thiz * k;
        }
        public static HexCoords2 operator /(HexCoords2 thiz, float k)
        {
            return thiz * (1 / k);
        }

        public static bool operator ==(HexCoords2 lhs, HexCoords2 rhs)
        {
            return (lhs.u == rhs.u) && (lhs.v == rhs.v);
        }
        public static bool operator !=(HexCoords2 lhs, HexCoords2 rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(u, v).GetHashCode();
        }

        public static readonly HexCoords2 zero = new HexCoords2(0, 0);
        public static readonly HexCoords2 one = new HexCoords2(1, 1);
        public static readonly HexCoords2 uPlus = new HexCoords2(1, 0);
        public static readonly HexCoords2 vPlus = new HexCoords2(0, 1);
        public static readonly HexCoords2 uMinus = new HexCoords2(-1, 0);
        public static readonly HexCoords2 vMinus = new HexCoords2(0, -1);
    }
}

using System;
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

        public static HexCoords3 operator +(HexCoords3 lhs, HexCoords3 rhs)
        {
            return new HexCoords3(lhs.u + rhs.u, lhs.v + rhs.v, lhs.y + rhs.y);
        }
        public static HexCoords3 operator -(HexCoords3 lhs, HexCoords3 rhs)
        {
            return lhs + (-rhs);
        }
        public static HexCoords3 operator -(HexCoords3 thiz)
        {
            return new HexCoords3(-thiz.u, -thiz.v, -thiz.y);
        }
        public static HexCoords3 operator *(HexCoords3 thiz, int k)
        {
            return new HexCoords3(thiz.u * k, thiz.v * k, thiz.y * k);
        }
        public static HexCoords3 operator *(int k, HexCoords3 thiz)
        {
            return thiz * k;
        }
        public static HexCoords3 operator /(HexCoords3 thiz, int k)
        {
            return thiz * (1/k);
        }
        public static HexCoords3 operator *(HexCoords3 thiz, float k)
        {
            return new HexCoords3(thiz.u * k, thiz.v * k, thiz.y * k);
        }
        public static HexCoords3 operator *(float k, HexCoords3 thiz)
        {
            return thiz * k;
        }
        public static HexCoords3 operator /(HexCoords3 thiz, float k)
        {
            return thiz * (1 / k);
        }

        public static bool operator ==(HexCoords3 lhs, HexCoords3 rhs)
        {
            return (lhs.u == rhs.u) && (lhs.v == rhs.v) && (lhs.y == rhs.y);
        }
        public static bool operator !=(HexCoords3 lhs, HexCoords3 rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(u, Tuple.Create(v, y)).GetHashCode();
        }

        public static readonly HexCoords3 zero = new HexCoords3(0, 0, 0);
        public static readonly HexCoords3 one = new HexCoords3(1, 1, 1);
        public static readonly HexCoords3 uPlus = new HexCoords3(1, 0, 0);
        public static readonly HexCoords3 vPlus = new HexCoords3(0, 1, 0);
        public static readonly HexCoords3 yPlus = new HexCoords3(0, 0, 1);
        public static readonly HexCoords3 uMinus = new HexCoords3(-1, 0, 0);
        public static readonly HexCoords3 vMinus = new HexCoords3(0, -1, 0);
        public static readonly HexCoords3 yMinus = new HexCoords3(0, 0, -1);
    }
}

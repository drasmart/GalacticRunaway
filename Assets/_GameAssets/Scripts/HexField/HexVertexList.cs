using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class HexVertexList
    {
        public List<HexCoords3Int> vertices = new List<HexCoords3Int>();
        public bool isLooped = false;

        public void Optimize()
        {
            if (vertices.Count <= 3)
            {
                return;
            }
            for(int i = (isLooped ? 0 : 1); i < vertices.Count - (isLooped ? 0 : 1); )
            {
                int n = vertices.Count;
                var v0 = (vertices[i] - vertices[(n + i - 1) % n]).ToVector3().normalized;
                var v1 = (vertices[(i + 1) % n] - vertices[i]).ToVector3().normalized;
                if (v0 == v1)
                {
                    vertices.RemoveAt(i);
                    continue;
                } else
                {
                    i++;
                }
            }
        }

        public void Revert()
        {
            var newList = new List<HexCoords3Int>(vertices.Count);
            for(int i = 0, n = vertices.Count; i < n; i++)
            {
                newList.Add(vertices[n - i - 1]);
            }
            vertices = newList;
        }
    }
}

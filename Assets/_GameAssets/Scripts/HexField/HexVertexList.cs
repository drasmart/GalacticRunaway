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
            for(int i = 1; i < vertices.Count - 1; )
            {
                var v0 = (vertices[i] - vertices[i - 1]).ToVector3().normalized;
                var v1 = (vertices[i + 1] - vertices[i]).ToVector3().normalized;
                if (v0 == v1)
                {
                    vertices.RemoveAt(i);
                    continue;
                } else
                {
                    i++;
                }
            }
            while (isLooped && vertices.Count > 2)
            {
                var v0 = (vertices[0] - vertices[vertices.Count - 1]).ToVector3().normalized;
                var v1 = (vertices[1] - vertices[0]).ToVector3().normalized;
                if (v0 == v1)
                {
                    vertices.RemoveAt(0);
                } else
                {
                    break;
                }
            }
        }
    }
}

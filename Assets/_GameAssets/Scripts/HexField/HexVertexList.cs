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

        }
    }
}

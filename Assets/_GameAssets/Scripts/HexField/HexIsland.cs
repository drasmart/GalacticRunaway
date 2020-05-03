using MatrixModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class HexIsland
    {
        public HexVertexList externalOutline = new HexVertexList();
        public List<HexVertexList> internalOutlines = new List<HexVertexList>();
    }
}

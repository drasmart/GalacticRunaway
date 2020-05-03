using HexGrid;
using MatrixModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace HexField
{
    public class HexNavMesh
    {
        public readonly bool[,] passableCells;
        public readonly HexCoords2Int cellsSize;
        public readonly HexCoords2Int edgesSize;

        [System.Serializable]
        public enum CellRole
        {
            Undefined = 0,
            External,
            OnMesh,
            Internal,
        }

        public HexNavMesh(BoolMatrix passableCells)
        {
            var srcSize = passableCells.Size;
            cellsSize = new HexCoords2Int(srcSize.x, srcSize.y);
            edgesSize = new HexCoords2Int(srcSize.x * 2 + 1, srcSize.y * 2 + 1);
            this.passableCells = passableCells.ToArray();
        }

        private CellRole[,] RolesMatrix
        {
            get {
                var result = new CellRole[cellsSize.u + 2, cellsSize.v + 2];
                for (int u = 0; u < cellsSize.u; u++)
                    for (int v = 0; v < cellsSize.v; v++)
                        if (passableCells[u, v])
                            result[u + 1, v + 1] = CellRole.OnMesh;

                UnityAction<HexCoords2Int, CellRole> Paint = (v, r) =>
                {
                    var cellsToPaint = new LinkedList<HexCoords2Int>();
                    cellsToPaint.AddLast(v);
                    while(cellsToPaint.Count > 0)
                    {
                        var nextCoords = cellsToPaint.First.Value;
                        cellsToPaint.RemoveFirst();
                        if (result[nextCoords.u, nextCoords.v] != CellRole.Undefined)
                        {
                            continue;
                        }
                        result[nextCoords.u, nextCoords.v] = r;
                        foreach (var testCoords in nextCoords.Neighbors)
                            if (0 <= testCoords.u && testCoords.u <= cellsSize.u + 1 && 0 <= testCoords.v && testCoords.v <= cellsSize.v + 1 && result[testCoords.u, testCoords.v] == CellRole.Undefined)
                                cellsToPaint.AddLast(testCoords);
                    }
                };
                Paint(HexCoords2Int.zero, CellRole.External);
                for (int u = 0; u < cellsSize.u + 2; u++)
                    for (int v = 0; v < cellsSize.v + 2; v++)
                        if (result[u, v] == CellRole.Undefined)
                            Paint(new HexCoords2Int(u, v), CellRole.Internal);
                return result;
            }
        }

        public CellRole[,] EdgesMatrix {
            get {
                var result = new CellRole[edgesSize.u, edgesSize.v];
                var rolesMatrix = RolesMatrix;
                var testOffsets = new HexCoords2Int[] { HexCoords2Int.uPlus, HexCoords2Int.one, HexCoords2Int.vPlus };
                for (int u = 0; u < cellsSize.u + 1; u++)
                    for (int v = 0; v < cellsSize.v + 1; v++)
                    {
                        var thisCell = rolesMatrix[u, v];
                        var coords = new HexCoords2Int(u, v);
                        foreach(var offset in testOffsets)
                        {
                            var nextCoords = coords + offset;
                            var nextCell = rolesMatrix[nextCoords.u, nextCoords.v];
                            if (nextCell == thisCell)
                            {
                                continue;
                            }
                            var edgeRole = (thisCell == CellRole.External || nextCell == CellRole.External) ? CellRole.External : CellRole.Internal;
                            var edgeCoords = (nextCoords - HexCoords2Int.one) * 2 - offset + HexCoords2Int.one;
                            result[edgeCoords.u, edgeCoords.v] = edgeRole;
                        }
                    }
                return result;
            }
        }
    }
}

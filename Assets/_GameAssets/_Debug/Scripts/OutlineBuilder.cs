using HexField;
using HexGrid;
using MatrixModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class OutlineBuilder : MonoBehaviour
{
    public OutlineRenderer src;
    public OutlineRenderer exts;
    public OutlineRenderer ints;
    public IslandRenderer isl;

    void Start()
    {
        BuildContourMatrix(src?.flags, exts?.flags, ints?.flags, isl?.island);
    }

    private void BuildContourMatrix(BoolMatrix cells, BoolMatrix externalEdges, BoolMatrix internalEdges, HexIsland island)
    {
        if (cells == null || externalEdges == null || internalEdges == null)
        {
            return;
        }
        var hexNavMesh = new HexNavMesh(cells);
        var edgesSize = new Vector2Int(hexNavMesh.edgesSize.u, hexNavMesh.edgesSize.v);
        externalEdges.Size = edgesSize;
        internalEdges.Size = edgesSize;
        var edgesMatrix = hexNavMesh.EdgesMatrix;
        for (int u = 0; u < edgesSize.x; u++)
            for (int v = 0; v < edgesSize.y; v++)
                switch (edgesMatrix[u, v])
                {
                    case HexNavMesh.CellRole.External:
                        externalEdges[new Vector2Int(u, v)] = true;
                        break;
                    case HexNavMesh.CellRole.Internal:
                        internalEdges[new Vector2Int(u, v)] = true;
                        break;
                    default:
                        break;
                }

        // Island calculations
        if (island == null)
        {
            return;
        }
        island.externalOutline.vertices.Clear();
        island.externalOutline.isLooped = true;
        island.internalOutlines.Clear();

        UnityAction<HexCoords2Int, HexNavMesh.CellRole, List<HexCoords3Int>> PopulateList = (v, r, l) =>
        {
            HexCoords2Int? nextCoords = v;
            do
            {
                var coords = nextCoords.Value;
                l.Add(coords);
                edgesMatrix[coords.u, coords.v] = HexNavMesh.CellRole.Undefined;
                nextCoords = null;
                foreach (var testCoords in coords.Neighbors)
                {
                    if (0 <= testCoords.u && testCoords.u < edgesSize.x && 0 <= testCoords.v && testCoords.v < edgesSize.y && edgesMatrix[testCoords.u, testCoords.v] == r)
                    {
                        nextCoords = testCoords;
                        break;
                    }
                }
            } while (nextCoords != null);
        };
        for (int u = 0; u < edgesSize.x; u++)
        {
            for (int v = 0; v < edgesSize.y; v++)
            {
                switch(edgesMatrix[u, v])
                {
                    case HexNavMesh.CellRole.External:
                        PopulateList(new HexCoords2Int(u, v), HexNavMesh.CellRole.External, island.externalOutline.vertices);
                        break;
                    case HexNavMesh.CellRole.Internal:
                        var newOutline = new HexVertexList();
                        newOutline.isLooped = true;
                        island.internalOutlines.Add(newOutline);
                        PopulateList(new HexCoords2Int(u, v), HexNavMesh.CellRole.Internal, newOutline.vertices);
                        break;
                }
            }
        }
    }
}

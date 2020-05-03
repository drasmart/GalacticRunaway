using HexField;
using MatrixModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CountourBuilder : MonoBehaviour
{
    public CountourRenderer src;
    public CountourRenderer exts;
    public CountourRenderer ints;

    void Start()
    {
        BuildContourMatrix(src?.flags, exts?.flags, ints?.flags);
    }

    private void BuildContourMatrix(BoolMatrix cells, BoolMatrix externalEdges, BoolMatrix internalEdges)
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
    }
}

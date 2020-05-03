using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixModels;
using HexGrid;

public class CountourRenderer : MonoBehaviour
{
    public BoolMatrix flags = new BoolMatrix();
    public IntMatrix nums = new IntMatrix();
    public Color sphereColor = Color.cyan;
    public Color boundsColor = Color.white;
    [Range(0, 1)]
    public float radius = 0.5f;
    [Min(0)]
    public float scale = 1;

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = sphereColor;
        var size = new Vector2Int(flags.Columns, flags.Rows);
        for (int rowIndex = 0; rowIndex < flags.Rows; rowIndex++)
            for(int columnIndex = 0; columnIndex < flags.Columns; columnIndex++)
                if (flags[new Vector2Int(columnIndex, rowIndex)])
                {
                    Gizmos.DrawWireSphere(new HexCoords2Int(columnIndex, rowIndex).ToVector3() * scale + transform.position, radius * scale);
                }
        Gizmos.color = boundsColor;
        DrawLimits();
        Gizmos.color = oldColor;
    }
    private void DrawLimits()
    {
        HexCoords2 uvm = -HexCoords2.one * 0.5f;
        HexCoords2 uvp = new HexCoords2(flags.Columns, flags.Rows) + uvm;

        HexCoords2 up = uvp; up.v = -0.5f;
        HexCoords2 vp = uvp; vp.u = -0.5f;
        HexCoords2 um = uvm; um.v = 0.5f;
        HexCoords2 vm = uvm; vm.u = 0.5f;

        Vector3 dy = Vector3.one * 0.01f;
        HexCoords2[] loop = new HexCoords2[] { up, uvp, vp, um, uvm, vm };
        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(loop[i].ToVector3() * scale + dy + transform.position, loop[(i + 1) % 6].ToVector3() * scale + dy + transform.position);
        }
    }
}

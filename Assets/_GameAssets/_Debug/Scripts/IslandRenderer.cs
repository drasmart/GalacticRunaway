using HexField;
using HexGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class IslandRenderer : MonoBehaviour
{
    public HexIsland island = new HexIsland();

    public Color externalOutlineColor = Color.cyan;
    public Color internalOutlineColor = Color.yellow;

    [Min(0)]
    public float scale = 1;
    [Range(0, 1)]
    public float radius = 0.5f;

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Vector3 dy = transform.position + 0.01f * Vector3.up;
        Gizmos.color = externalOutlineColor;
        foreach (var vertex in island.externalOutline.vertices)
        {
            var coords = vertex.ToVector3() * scale + dy;
            Gizmos.DrawWireSphere(coords, radius * scale);
        }
        Gizmos.color = internalOutlineColor;
        foreach (var nextOutline in island.internalOutlines)
            foreach (var vertex in nextOutline.vertices)
            {
                var coords = vertex.ToVector3() * scale + dy;
                Gizmos.DrawWireSphere(coords, radius * scale);
            }
        Gizmos.color = oldColor;
    }

    public void RefreshMesh()
    {
        if(meshFilter)
        {
            meshFilter.mesh = island.BuildMesh();
        }
    }
}

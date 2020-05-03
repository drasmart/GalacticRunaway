using MatrixModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace HexField
{
    [System.Serializable]
    public class HexIsland
    {
        public HexVertexList externalOutline = new HexVertexList();
        public List<HexVertexList> internalOutlines = new List<HexVertexList>();

        public Mesh BuildMesh()
        {
            var vertices = externalOutline.ToVector3List();
            var polygon = new List<int>(vertices.Count + 2 * internalOutlines.Count + internalOutlines.Aggregate(0, (s, o) => s + o.vertices.Count));
            for(int i = 0; i < vertices.Count; i++)
            {
                polygon.Add(i);
            }

            UnityAction<List<Vector3>> InjectHole = subList =>
            {
                int srcIndex = 0;
                int dstIndex = 0;
                float d = Mathf.Infinity;
                int srcN = subList.Count;
                int dstN = polygon.Count;
                int srcVertexOffset = vertices.Count;
                for (int j = 0; j < srcN; j++)
                {
                    var v = subList[j];
                    vertices.Add(subList[j]);
                    for (int i = 0; i < dstN; i++)
                    {
                        var l = (vertices[polygon[i]] - subList[j]).sqrMagnitude;
                        if (l >= d)
                        {
                            continue;
                        }
                        var iv0 = polygon[(dstN + i - 1) % dstN];
                        var iv1 = polygon[i];
                        var iv2 = polygon[(i + 1) % dstN];
                        var v0 = vertices[iv0];
                        var v1 = vertices[iv1];
                        var v2 = vertices[iv2];
                        var s1 = Vector3.Cross(v1 - v0, v2 - v1).y > 0;
                        var s = Vector3.Cross(v1 - v0, v - v1).y > 0;
                        if (s1 != s)
                        {
                            continue;
                        }
                        d = l;
                        srcIndex = j;
                        dstIndex = i;
                    }
                }
                var insertionPoint = dstIndex + 1;
                polygon.Insert(insertionPoint, polygon[dstIndex]);
                for(int j = 0; j <= srcN; j++)
                {
                    int k = (j + srcIndex) % srcN;
                    polygon.Insert(insertionPoint + j, srcVertexOffset + k);
                }

                string msg = "";
                for (int i = 0; i < vertices.Count; i++)
                    msg += vertices[i].ToString() + "\n";
                for (int i = 0; i < polygon.Count; i++)
                    msg += polygon[i].ToString() + " ";
                Debug.Log(msg);
            };

            foreach(var nextList in internalOutlines)
            {
                InjectHole(nextList.ToVector3List());
            }

            var tris = new List<int>();
            UnityAction<int> ClipEar = vIndex =>
            {
                Debug.Log("Clipping vertex #" + polygon[vIndex].ToString() + " (@" + vIndex.ToString() + "): " + vertices[polygon[vIndex]].ToString());
                int pn = polygon.Count;
                tris.Add(polygon[(vIndex + 1) % pn]);
                tris.Add(polygon[vIndex]);
                tris.Add(polygon[(pn + vIndex - 1) % pn]);
                polygon.RemoveAt(vIndex);
            };

            while(polygon.Count > 3)
            {
                int cnt = polygon.Count;
                bool clipped = false;
                for(int i = 0; i < cnt; i++)
                {
                    var iv0 = polygon[(cnt + i - 1) % cnt];
                    var iv1 = polygon[i];
                    var iv2 = polygon[(i + 1) % cnt];
                    var v0 = vertices[iv0];
                    var v1 = vertices[iv1];
                    var v2 = vertices[iv2];
                    var s1 = -Vector3.Cross(v1 - v0, v2 - v1).y;
                    if (s1 < 0)
                    {
                        continue;
                    }
                    var linearFactors = new List<float>();
                    UnityAction<Vector3, Vector3, List<float>> AddNums = (a, b, y) =>
                    {
                        y.Add(a.z - b.z);
                        y.Add(b.x - a.x);
                        y.Add(a.x * b.z - a.z * b.x);
                    };
                    Func<Vector3, List<float>, int, int> EvalSign = (v, y, o) =>
                    {
                        float q = (y[o * 3] * v.x + y[o * 3 + 1] * v.z + y[o * 3 + 2]);
                        return (q > 0) ? 1 : ((q < 0) ? -1 : 0);
                    };
                    AddNums(v0, v1, linearFactors);
                    AddNums(v0, v2, linearFactors);
                    AddNums(v1, v2, linearFactors);
                    Func<Vector3, Vector3, bool> EvalIntersect = (a, b) =>
                    {
                        for (int o = 0; o < 3; o++)
                            if (EvalSign(a, linearFactors, o) != EvalSign(b, linearFactors, o))
                            {
                                var linF2 = new List<float>();
                                AddNums(a, b, linF2);
                                if (EvalSign(v0, linF2, 0) != EvalSign(v1, linF2,0) 
                                || EvalSign(v0, linF2, 0) != EvalSign(v2, linF2, 0) 
                                || EvalSign(v1, linF2, 0) != EvalSign(v2, linF2, 0))
                                    return true;
                            }
                        return false;
                    };
                    bool isEar = true;
                    for(int j = 0; j < cnt - 3; j++)
                    {
                        int k = (i + 2 + j) % cnt;
                        var ik = polygon[k];
                        var v = vertices[ik];
                        if (ik == iv0 || ik == iv1 || ik == iv2)
                        {
                            continue;
                        }
                        var s = Vector3.Cross(v - v0, v1 - v).magnitude + Vector3.Cross(v - v1, v2 - v).magnitude + Vector3.Cross(v - v0, v2 - v).magnitude;
                        if (Mathf.Approximately(Mathf.Abs(s1), s))
                        {
                            isEar = false;
                            break;
                        }
                        if (j < cnt - 4)
                        {
                            int k2 = (i + 2 + j + 1) % cnt;
                            var ik2 = polygon[k2];
                            var w = vertices[polygon[k2]];
                            if (!(ik2 == iv0 || ik2 == iv1 || ik2 == iv2) && EvalIntersect(v, w))
                            {
                                isEar = false;
                                break;
                            }
                        }
                    }
                    if (isEar)
                    {
                        ClipEar(i);
                        clipped = true;
                        break;
                    }
                }
                if (!clipped)
                {
                    Debug.LogWarning("Failed to clip ear");
                    break;
                }
            }
            if (polygon.Count == 3)
            {
                ClipEar(0);
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = vertices.Select(v => new Vector2(v.x ,v.z)).ToArray();
            mesh.triangles = tris.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

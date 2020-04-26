using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    public class BattleFieldManager : MonoBehaviour
    {
        public BattleField battleField = new BattleField();

        #region Debug
        [Header("Debug")]
        public GizmoColors gizmoColors = new GizmoColors();

        [System.Serializable]
        public class GizmoColors
        {
            public List<Color> sides = new List<Color>(new Color[] { Color.cyan, Color.red });
            public Color pickups = Color.yellow;
            public Color obstacles = Color.gray;
            public Color limits = Color.white;
        }

        public void OnDrawGizmos()
        {
            Color oldColor = Gizmos.color;
            for (int i = 0, n = Mathf.Max(battleField.sides.Count, gizmoColors.sides.Count); i < n; i++)
            {
                Gizmos.color = gizmoColors.sides[i];
                foreach (var unit in battleField.sides[i].units)
                {
                    DrawMarker(unit.coords, unit.rotation);
                }
            }
            Gizmos.color = gizmoColors.pickups;
            foreach (var pickup in battleField.lootBoxes)
            {
                DrawMarker(pickup.coords, null);
            }
            Gizmos.color = gizmoColors.obstacles;
            foreach (var obstacle in battleField.obstacles)
            {
                DrawMarker(obstacle.coords, null);
            }
            Gizmos.color = gizmoColors.limits;
            DrawLimits();
            Gizmos.color = oldColor;
        }

        private void DrawMarker(HexCoords2Int coords, float? rotation)
        {
            const float r = 0.45f;
            const float l = 0.7f * r;
            const float da = 45;
            const float ay = 0.1f;
            var p = coords.ToVector3();
            var pa = p + Vector3.up * ay;
            Gizmos.DrawWireSphere(pa, r);
            if (rotation != null)
            {
                var a = rotation.Value;
                var fwd = Quaternion.Euler(0, a, 0) * Vector3.forward * l;
                var sideL = Quaternion.Euler(0, a + 180 - da, 0) * Vector3.forward * l;
                var sideR = Quaternion.Euler(0, a + 180 + da, 0) * Vector3.forward * l;
                var bck = Quaternion.Euler(0, a, 0) * Vector3.back * l * 0.5f;
                Gizmos.DrawLine(pa + sideL, pa + fwd);
                Gizmos.DrawLine(pa + sideR, pa + fwd);
                Gizmos.DrawLine(pa + sideL, pa + bck);
                Gizmos.DrawLine(pa + sideR, pa + bck);
            }
        }

        private void DrawLimits()
        {
            HexCoords2 uvp = HexCoords2.one * (battleField.radius - 0.5f);
            HexCoords2 uvm = -uvp;

            HexCoords2 up = uvp; up.v = 0;
            HexCoords2 vp = uvp; vp.u = 0;
            HexCoords2 um = uvm; um.v = 0;
            HexCoords2 vm = uvm; vm.u = 0;

            HexCoords2 dp = -battleField.offset;

            Vector3 dy = Vector3.one * 0.01f;
            HexCoords2[] loop = new HexCoords2[] { up, uvp, vp, um, uvm, vm };
            for(int i = 0; i < 6; i++)
            {
                Gizmos.DrawLine((loop[i] + dp).ToVector3() + dy, (loop[(i + 1) % 6] + dp).ToVector3() + dy);
            }
        }
        #endregion
    }
}

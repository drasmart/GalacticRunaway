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
            public Color obstacles = Color.white;
        }

        public void OnDrawGizmos()
        {
            Debug.Log("1");
            const float r = 0.5f;
            Vector3 dy = Vector3.up * r;
            Color oldColor = Gizmos.color;
            for (int i = 0, n = Mathf.Max(battleField.sides.Count, gizmoColors.sides.Count); i < n; i++)
            {
                Gizmos.color = gizmoColors.sides[i];
                foreach (var unit in battleField.sides[i].units)
                {
                    Gizmos.DrawWireSphere(unit.coords.ToVector3() + dy, r);
                }
            }
            Gizmos.color = gizmoColors.pickups;
            foreach (var pickup in battleField.lootBoxes)
            {
                Gizmos.DrawWireSphere(pickup.coords.ToVector3() + dy, r);
            }
            Gizmos.color = gizmoColors.obstacles;
            foreach (var obstacle in battleField.obstacles)
            {
                Gizmos.DrawWireSphere(obstacle.coords.ToVector3() + dy, r);
            }
            Gizmos.color = oldColor;
        }
        #endregion
    }
}

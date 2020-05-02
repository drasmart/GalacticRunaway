using HexGrid;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

namespace HexField
{
    public class BattleFieldManager : MonoBehaviour
    {
        public BattleField battleField = new BattleField();

        [System.Serializable] public class BattleFieldMutationEvent : UnityEvent<BattleFieldMutation> { }
        [System.Serializable] public class BattleFieldResizeEvent : UnityEvent<int> { }

        public BattleFieldMutationEvent onBattleFieldMutation;
        public BattleFieldResizeEvent onBattleFieldResize;

        private void Start()
        {
            onBattleFieldResize?.Invoke(battleField.radius);
            if (onBattleFieldMutation != null)
            {
                for (int i = 0; i < battleField.sides.Count; i++)
                    foreach (var unit in battleField.sides[i].units)
                        onBattleFieldMutation.Invoke(new BattleFieldMutation(unit, unit.coords, i));
                foreach (var obstacle in battleField.obstacles)
                    onBattleFieldMutation.Invoke(new BattleFieldMutation(obstacle));
                foreach (var lootBox in battleField.lootBoxes)
                    onBattleFieldMutation.Invoke(new BattleFieldMutation(lootBox));
            }
        }

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
                    DrawMarker(unit);
                }
            }
            Gizmos.color = gizmoColors.pickups;
            battleField.lootBoxes.ForEach(DrawMarker);
            Gizmos.color = gizmoColors.obstacles;
            battleField.obstacles.ForEach(DrawMarker);
            Gizmos.color = gizmoColors.limits;
            DrawLimits();
            Gizmos.color = oldColor;
        }

        private void DrawMarker(BattleFieldElement element)
        {
            const float r = 0.45f;
            const float l = 0.7f * r;
            const float da = 45;
            const float ay = 0.1f;

            var rotation = element.rotation;
            var p = element.coords.ToVector3();

            var pa = p + Vector3.up * ay + transform.position;
            Gizmos.DrawWireSphere(pa, r);
            var fwd = Quaternion.Euler(0, rotation, 0) * Vector3.forward * l;
            var sideL = Quaternion.Euler(0, rotation + 180 - da, 0) * Vector3.forward * l;
            var sideR = Quaternion.Euler(0, rotation + 180 + da, 0) * Vector3.forward * l;
            var bck = Quaternion.Euler(0, rotation, 0) * Vector3.back * l * 0.5f;
            Gizmos.DrawLine(pa + sideL, pa + fwd);
            Gizmos.DrawLine(pa + sideR, pa + fwd);
            Gizmos.DrawLine(pa + sideL, pa + bck);
            Gizmos.DrawLine(pa + sideR, pa + bck);
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

            Vector3 dy = Vector3.one * 0.01f + transform.position;
            HexCoords2[] loop = new HexCoords2[] { up, uvp, vp, um, uvm, vm };
            for(int i = 0; i < 6; i++)
            {
                Gizmos.DrawLine((loop[i] + dp).ToVector3() + dy, (loop[(i + 1) % 6] + dp).ToVector3() + dy);
            }
        }
        #endregion

        #region Query API
        public IEnumerable<BattleFieldElement> GetElementsAt(HexCoords2Int coords)
        {
            foreach (var side in battleField.sides)
                foreach (var unit in side.units)
                    if (unit.coords == coords)
                        yield return unit;
            foreach (var obstacle in battleField.obstacles)
                if (obstacle.coords == coords)
                    yield return obstacle;
            foreach (var lootBox in battleField.lootBoxes)
                if (lootBox.coords == coords)
                    yield return lootBox;
            yield break;
        }
        public int GetUnitSide(LandUnit unit)
        {
            for (int i = 0; i < battleField.sides.Count; i++)
                if (battleField.sides[i].units.Contains(unit))
                    return i;
            return 0;
        }
        #endregion

        #region Mutation API
        public void MoveUnit(LandUnit landUnit, HexCoords2Int newCoords, int rotation)
        {
            var oldCoords = landUnit.coords;
            landUnit.coords = newCoords;
            landUnit.rotation = rotation;
            var mutation = new BattleFieldMutation(landUnit, oldCoords, GetUnitSide(landUnit));
            onBattleFieldMutation?.Invoke(mutation);
        }
        public void UpdateUnitStats(LandUnit landUnit, LandUnitStats newStats)
        {
            landUnit.stats = newStats;
            var mutation = new BattleFieldMutation(landUnit, landUnit.coords, GetUnitSide(landUnit));
            if (newStats.hp <= 0)
            {
                foreach (var side in battleField.sides)
                {
                    side.units.Remove(landUnit);
                }
            }
            onBattleFieldMutation?.Invoke(mutation);
        }
        public void DamageObstacle(LandObstacle obstacle, int damage)
        {
            obstacle.hp -= damage;
            if (obstacle.hp <= 0)
            {
                battleField.obstacles.Remove(obstacle);
            }
            var mutation = new BattleFieldMutation(obstacle);
            onBattleFieldMutation?.Invoke(mutation);
        }
        public void CollectLootBox(LootBox lootBox, int sideIndex)
        {
            if (0 <= sideIndex && sideIndex < battleField.sides.Count)
            {
                battleField.sides[sideIndex].points += lootBox.points;
            }
            battleField.lootBoxes.Remove(lootBox);
            lootBox.points = 0;
            var mutation = new BattleFieldMutation(lootBox);
            onBattleFieldMutation?.Invoke(mutation);
        }
        #endregion
    }
}

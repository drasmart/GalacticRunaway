using Boo.Lang.Environments;
using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    public class BattleFieldBuilder : MonoBehaviour
    {
        public BattleFieldElements elements;
        public Transform floorPlane;

        private Dictionary<BattleFieldElementType, Dictionary<HexCoords2Int, List<BattleFieldElementUI>>> createdUIs = new Dictionary<BattleFieldElementType, Dictionary<HexCoords2Int, List<BattleFieldElementUI>>>();

        public void OnFieldResize(int newRadius)
        {
            if (floorPlane == null)
            {
                return;
            }
            floorPlane.localScale = Vector3.one * (newRadius - 0.5f);
        }

        public void OnBattleFieldMutation(BattleFieldMutation mutation)
        {
            Debug.Log(mutation.ToString());
            var ui = FindUI(mutation);
            bool unregister = false;
            HexCoords2Int? newCoords = null;
            switch(mutation.elementType)
            {
                case BattleFieldElementType.Unit:
                    bool dead = (mutation.unit.stats.hp <= 0);
                    bool moved = !dead && (mutation.unit.coords != mutation.oldCoords);
                    unregister = dead || moved;
                    if (moved)
                    {
                        newCoords = mutation.unit.coords;
                    }
                    break;
                case BattleFieldElementType.Obstacle:
                    unregister = (mutation.obstacle.hp <= 0);
                    break;
                case BattleFieldElementType.LootBox:
                    unregister = (mutation.lootBox.points <= 0);
                    break;
            }
            if (ui != null && unregister)
            {
                UnregisterUI(mutation, ui);
            }
            if (ui == null && (!unregister || newCoords != null))
            {
                ui = CreateUI(mutation);
                RegisterUI(mutation, ui);
            }
            if (ui != null)
            {
                if (unregister && newCoords == null)
                {
                    Recycle(ui.gameObject);
                } else
                {
                    ui.RefreshUI();
                }
            }
        }
        private void RegisterUI(BattleFieldMutation mutation, BattleFieldElementUI ui)
        {
            Dictionary<HexCoords2Int, List<BattleFieldElementUI>> uisDic = null;
            if (createdUIs.ContainsKey(mutation.elementType))
            {
                uisDic = createdUIs[mutation.elementType];
            }
            else
            {
                uisDic = new Dictionary<HexCoords2Int, List<BattleFieldElementUI>>();
                createdUIs.Add(mutation.elementType, uisDic);
            }
            var coords = mutation.element.coords;
            List <BattleFieldElementUI> uisList = null;
            if (uisDic.ContainsKey(coords))
            {
                uisList = uisDic[coords];
            } else
            {
                uisList = new List<BattleFieldElementUI>();
                uisDic.Add(coords, uisList);
            }
            uisList.Add(ui);
        }
        private void UnregisterUI(BattleFieldMutation mutation, BattleFieldElementUI ui)
        {
            var coords = mutation.oldCoords;
            var uisList = createdUIs[mutation.elementType][coords];
            uisList.Remove(ui);
        }
        private BattleFieldElementUI FindUI(BattleFieldMutation mutation)
        {
            if (!createdUIs.ContainsKey(mutation.elementType))
            {
                return null;
            }
            var uisDic = createdUIs[mutation.elementType];
            if (!uisDic.ContainsKey(mutation.oldCoords))
            {
                return null;
            }
            var uisList = uisDic[mutation.oldCoords];
            foreach(var ui in uisList)
            {
                if (ui.modelElement == mutation.element)
                {
                    return ui;
                }
            }
            return null;
        }
        private BattleFieldElementUI CreateUI(BattleFieldMutation mutation)
        {
            var prefab = FindPrefab(mutation);
            if (prefab == null)
            {
                return null;
            }
            var clone = Fabricate(prefab);
            var hexTransform = clone.GetComponent<HexTransform>();
            if (hexTransform == null)
            {
                hexTransform = clone.AddComponent<HexTransform>();
            }
            var result = clone.GetComponent<BattleFieldElementUI>();
            if (result == null)
            {
                result = clone.AddComponent<BattleFieldElementUI>();
            }
            result.modelElement = mutation.element;
            clone.transform.SetParent(transform, false);
            return result;
        }
        private GameObject FindPrefab(BattleFieldMutation mutation)
        {
            if (elements == null)
            {
                return null;
            }
            switch(mutation.elementType)
            {
                case BattleFieldElementType.LootBox:
                    return elements.lootBox;
                case BattleFieldElementType.Obstacle:
                    return elements.obstacle;
                case BattleFieldElementType.Unit:
                    if (0 <= mutation.side && mutation.side < (elements.sideUnits?.Count ?? 0))
                        return elements.sideUnits[mutation.side];
                    return null;
                default:
                    return null;
            }
        }

        #region Prefab Pooling
        // TODO: Implement
        private GameObject Fabricate(GameObject prefab)
        {
            return Instantiate(prefab);
        }
        private void Recycle(GameObject target)
        {
            Destroy(target);
        }
        #endregion
    }
}

using HexField.UI;
using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [RequireComponent(typeof(HexTransform))]
    public class BattleFieldElementUI: MonoBehaviour
    {
        public BattleFieldElement modelElement;
        private HexTransform hexTransform;

        public HealthBar healthBar;

        private void Awake()
        {
            hexTransform = GetComponent<HexTransform>();
        }

        public void RefreshUI()
        {
            hexTransform.coords = (HexCoords3Int)modelElement.coords;
            hexTransform.UpdatePosition();
            transform.eulerAngles = Vector3.up * modelElement.rotation;
            var healthRange = (modelElement as Destructable)?.hpRange;
            if (healthBar != null && healthRange != null)
            {
                healthBar.UpdateHealth(healthRange.Item1, healthRange.Item2);
            }
        }
    }
}

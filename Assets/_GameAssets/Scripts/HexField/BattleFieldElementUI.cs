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

        private void Awake()
        {
            hexTransform = GetComponent<HexTransform>();
        }

        public void RefreshUI()
        {
            hexTransform.coords = (HexCoords3Int)modelElement.coords;
            hexTransform.UpdatePosition();
            transform.eulerAngles = Vector3.up * modelElement.rotation;
        }
    }
}

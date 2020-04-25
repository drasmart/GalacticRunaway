using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    public class HexTransform : MonoBehaviour
    {
        public HexCoords3 coords;

        private void OnValidate()
        {
            UpdatePosition();
        }
        public void UpdateCoords()
        {
            coords = new HexCoords3(transform.position);
        }
        public void UpdatePosition()
        {
            transform.position = coords.ToVector3();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    public class HexTransform : MonoBehaviour
    {
        public HexCoords3 coords;

        public void OnValidate()
        {
            transform.position = coords.toVector3();
        }
    }
}

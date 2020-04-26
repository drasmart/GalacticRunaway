using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraControls
{
    public class ZoomControl : MonoBehaviour
    {
        [Range(0, 100)]
        public float minDistance = 5;
        [Range(0, 100)]
        public float maxDistance = 25;
        [Range(0, 100)]
        public float sensitivity = 1.0f;

        private void Update()
        {
            float dz = Input.GetAxis("Mouse ScrollWheel");
            if (dz == 0)
            {
                return;
            }
            var p = transform.localPosition;
            p.z = -Mathf.Clamp(-p.z - dz * sensitivity, minDistance, maxDistance);
            transform.localPosition = p;
        }
    }
}

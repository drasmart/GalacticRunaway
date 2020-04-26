using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraControls
{
    public class VerticalRotor : MonoBehaviour
    {
        [Range(10, 90)]
        public float stepAngle = 60;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.eulerAngles += Vector3.up * stepAngle;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.eulerAngles -= Vector3.up * stepAngle;
            }
        }
    }
}

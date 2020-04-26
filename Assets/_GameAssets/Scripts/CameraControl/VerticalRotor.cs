using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CameraControls
{
    public class VerticalRotor : MonoBehaviour
    {
        [Range(10, 90)]
        public float stepAngle = 60;

        [System.Serializable]
        public class CameraRotatedEvent : UnityEvent<float> { }

        public CameraRotatedEvent onCameraRotated;

        public static VerticalRotor Instance => Camera.main?.GetComponentInParent<VerticalRotor>();

        void Update()
        {
            bool rotated = false;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.eulerAngles += Vector3.up * stepAngle;
                rotated = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.eulerAngles -= Vector3.up * stepAngle;
                rotated = true;
            }
            if (rotated)
            {
                onCameraRotated?.Invoke(transform.eulerAngles.y);
            }
        }
    }
}

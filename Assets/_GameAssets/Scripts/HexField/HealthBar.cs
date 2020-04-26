using CameraControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexField.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Image barImage;
        public Text text;
        public Gradient barColors;

        [Min(0)] public float hp = 50;
        [Min(0)] public float maxHP = 100;

        [Min(0)] public float widthScale = 1;

        public void UpdateHealth(float hp, float maxHP)
        {
            this.hp = hp;
            this.maxHP = maxHP;
            RefreshUI();
        }

        private void OnEnable()
        {
            var rotor = VerticalRotor.Instance;
            if (rotor)
            {
                rotor.onCameraRotated.AddListener(UpdateRotation);
                UpdateRotation(rotor.transform.eulerAngles.y);
            }
        }
        private void OnDisable()
        {
            var rotor = CameraControls.VerticalRotor.Instance;
            if (rotor)
            {
                rotor.onCameraRotated.RemoveListener(UpdateRotation);
            }
        }
        private void OnValidate()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            UpdateRotation(VerticalRotor.Instance.transform.eulerAngles.y);
            if (text != null)
            {
                text.text = hp.ToString() + "/" + maxHP.ToString();
            }
            if (barImage == null)
            {
                return;
            }
            float maxVal = (maxHP > 0) ? maxHP : 1;
            float t = Mathf.Clamp(hp / maxVal, 0, 1);
            barImage.color = barColors.Evaluate(t);
            (barImage.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthScale * t);
        }
        private void UpdateRotation(float rotation)
        {
            transform.eulerAngles = Vector3.up * rotation;
        }
    }
}

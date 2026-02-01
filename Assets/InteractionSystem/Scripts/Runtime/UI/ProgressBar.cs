using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.Scripts.Runtime.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image fillImage;

        [Header("Values")]
        [SerializeField] private float maxValue = 100f;
        [SerializeField] private float currentValue;
        [SerializeField] private float addedValue = .2f;

        [Header("Settings")]
        [SerializeField] private bool smooth = true;
        [SerializeField] private float lerpSpeed = 10f;

        public event Action OnProgressComplete;


        private void Awake()
        {
            SetValue(0);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                AddValue(addedValue);
            }
           
        }

       
        public void SetMaxValue(float value)
        {
        }

        public void SetValue(float value)
        {

        }

        public void AddValue(float value)
        {
            if(currentValue >= maxValue)
            {
                OnProgressComplete?.Invoke();
                return;
            }

            float addValue = value / 100;

            fillImage.fillAmount += addValue * Time.deltaTime;
            currentValue = fillImage.fillAmount * 100;
           
        }

        public void ResetBar()
        {
        }

        public float GetNormalizedValue()
        {
            return currentValue / maxValue;
        }


    }
}
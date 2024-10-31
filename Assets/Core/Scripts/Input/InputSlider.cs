using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BallBlust.Input
{
    public class InputSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action<float> OnSliderInput;

        [SerializeField] private Slider _inputSlider;
        [SerializeField] private Vector2 _inputBounds;

        public bool IsPressed { get; private set; }


        private void OnEnable() => _inputSlider.onValueChanged.AddListener(OnValueChanged);
        private void OnDisable() => _inputSlider.onValueChanged.RemoveListener(OnValueChanged);

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;

        private void OnValueChanged(float value)
        {
            var boundedValue = Mathf.InverseLerp(_inputBounds.x, _inputBounds.y, value);
            OnSliderInput.Invoke(boundedValue);
        }
    }
}

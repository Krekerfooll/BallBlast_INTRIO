using TMPro;
using UnityEngine;

namespace BallBlust.Core
{
    public class Cell : BaseCell
    {
        [Space]
        [SerializeField] private TextMeshPro _counter;
        [SerializeField] private Vector2 _counterSizeMultiplier;

        protected override void OnInit(Vector2 size, int startHP)
        {
            _counter.rectTransform.sizeDelta = size * _counterSizeMultiplier;
        }

        protected override void OnCurrentHPChanged(int value)
        {
            base.OnCurrentHPChanged(value);
            _counter.text = value.ToString();
        }
    }
}

using UnityEngine;

namespace BallBlust.Core
{
    public class BonusCell : BaseCell
    {
        [Space]
        [SerializeField] private SpriteRenderer _innerBack;
        [SerializeField] private SpriteRenderer _iconFade;
        [SerializeField] private SpriteRenderer _bonusIcon;
        [SerializeField] private Vector2 _innerBackPadding;
        [SerializeField] private float _iconFadePadding;
        [SerializeField] private float _iconSizePadding;

        protected override void OnInit(Vector2 size, int startHP)
        {
            var iconSize = Mathf.Min(size.x, size.y) - _iconSizePadding;
            var fadeSize = Mathf.Min(size.x, size.y) - _iconFadePadding;

            _bonusIcon.size = new Vector2(iconSize, iconSize);
            _iconFade.size = new Vector2(fadeSize, fadeSize);
            _innerBack.size = size - _innerBackPadding;
        }
    }
}

using UnityEngine;

namespace BallBlust.Core
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private Collider2D _shieldCollider;
        [SerializeField] private SpriteRenderer _shieldVisual;
        [SerializeField] private Vector2 _shieldAlphaBounds;

        private int _maxHP;

        private void Start()
        {
            _maxHP = CannonStatsController.CurrentStats.MaxShieldHP;
        }

        private void Update()
        {
            var currentHP = CannonStatsController.CurrentStats.ShieldHP;
            var lerpValue = Mathf.InverseLerp(0, _maxHP, currentHP);
            var alphaValue = Mathf.Lerp(_shieldAlphaBounds.x, _shieldAlphaBounds.y, lerpValue);

            _shieldVisual.color = new Color(_shieldVisual.color.r, _shieldVisual.color.g, _shieldVisual.color.b, alphaValue);
            _shieldCollider.enabled = currentHP > 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(collision.gameObject);
            CannonStatsController.AddShieldHP(-1);
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace BallBlust.Core
{
    public class Projectile : MonoBehaviour
    {
        private int _damage;

        public void Lounch(int damage, float projectileSpeed, float distance)
        {
            _damage = damage;

            var targetHeight = transform.position.y + distance;
            var duration = distance / projectileSpeed;

            transform.DOMoveY(targetHeight, duration)
                     .OnComplete(() => Destroy(gameObject));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Cell>(out var cell)) 
            {
                transform.DOKill();

                cell.TakeDamage(_damage);

                Destroy(gameObject);
            }
        }
    }
}

using DG.Tweening;
using System;
using UnityEngine;

namespace BallBlust.Core
{
    public class Projectile : MonoBehaviour
    {
        public Action<Projectile> OnProjectileDestroy;

        [SerializeField] private int _damage;

        public void Lounch(float projectileSpeed, float distance)
        {
            var targetHeight = transform.position.y + distance;
            var duration = distance / projectileSpeed;

            transform.DOMoveY(targetHeight, duration)
                     .OnComplete(() => OnProjectileDestroy?.Invoke(this));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<BaseCell>(out var cell)) 
            {
                transform.DOKill();
                cell.TakeDamage(_damage);
                OnProjectileDestroy?.Invoke(this);
            }
        }
    }
}

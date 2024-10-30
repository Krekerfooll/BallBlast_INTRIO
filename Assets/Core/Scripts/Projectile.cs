using DG.Tweening;
using UnityEngine;

namespace BallBlust.Core
{
    public class Projectile : MonoBehaviour
    {
        public void Lounch(float projectileSpeed, float distance)
        {
            var targetHeight = transform.position.y + distance;
            var duration = distance / projectileSpeed;

            transform.DOMoveY(targetHeight, duration)
                     .OnComplete(() => Destroy(gameObject));
        }
    }
}

using UnityEngine;

namespace BallBlust.Core.Drop
{
    public class DroppableFireRateModifier : DroppablePoolingObject
    {
        [SerializeField] private float _fireRateModifier;

        protected override void OnCollideWithGround(Collision2D collision)
        {

        }

        protected override void OnCollideWithPlayer(Collision2D collision)
        {
            CannonStatsController.AddFireRate(_fireRateModifier);
            ReturnToPool();
        }
    }
}

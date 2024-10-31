using UnityEngine;

namespace BallBlust.Core.Drop
{
    public class DroppableShieldModifier : DroppablePoolingObject
    {
        [SerializeField] private int _shieldHP;

        protected override void OnCollideWithGround(Collision2D collision)
        {

        }

        protected override void OnCollideWithPlayer(Collision2D collision)
        {
            CannonStatsController.AddShieldHP(_shieldHP);
            ReturnToPool();
        }
    }
}

using UnityEngine;

namespace BallBlust.Core.Drop
{
    public class DroppableCoin : DroppablePoolingObject
    {
        [SerializeField] private int _amount;

        protected override void OnCollideWithGround(Collision2D collision)
        {
            
        }

        protected override void OnCollideWithPlayer(Collision2D collision)
        {
            ReturnToPool();
        }
    }
}

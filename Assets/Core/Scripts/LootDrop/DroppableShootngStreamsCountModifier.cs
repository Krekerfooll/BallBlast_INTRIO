using UnityEngine;

namespace BallBlust.Core.Drop
{
    public class DroppableShootngStreamsCountModifier : DroppablePoolingObject
    {
        [SerializeField] private int _addStreamCount;

        protected override void OnCollideWithGround(Collision2D collision)
        {

        }

        protected override void OnCollideWithPlayer(Collision2D collision)
        {
            CannonStatsController.AddStreamsCount(_addStreamCount);
            ReturnToPool();
        }
    }
}

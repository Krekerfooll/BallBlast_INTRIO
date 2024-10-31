using UnityEngine;

namespace BallBlust.Core.Drop
{
    public abstract class DroppablePoolingObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public LootDropHelper.DropType DropType { get; private set; }
        public Rigidbody2D Rigidbody => _rigidbody;

        public void Init(LootDropHelper.DropType dropType)
        {
            DropType = dropType;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ground":
                    OnCollideWithGround(collision);
                    break;

                case "Player":
                    OnCollideWithPlayer(collision);
                    break;
            }
        }

        protected abstract void OnCollideWithGround(Collision2D collision);
        protected abstract void OnCollideWithPlayer(Collision2D collision);

        public virtual void ReturnToPool()
        {
            OnReturnToPool();
            LootDropHelper.Instance.ReturnToPool(this);
        }

        public virtual void OnGetFromPool() 
        {
            gameObject.SetActive(true);
        }
        public virtual void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
}

using BallBlust.Core.Drop;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BallBlust.Core
{
    public abstract class BaseCell : MonoBehaviour
    {
        public Action<int> OnCellHPChanged;
        public Action<BaseCell> OnCellDestroyed;

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Vector2 _colliderSizeMultiplier;
        [SerializeField] private DropData[] _drop;

        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            protected set
            {
                _currentHP = value;
                OnCurrentHPChanged(value);
                
                if (value <= 0 && !_isDestroyed)
                {
                    _isDestroyed = true;
                    DestroyCell();
                }
            }
        }

        private bool _isDestroyed;
        private bool _isCloseToLeftBound;

        public void Init(Vector2 size, int startHP, bool isCloseToLeftBound)
        {
            _isCloseToLeftBound = isCloseToLeftBound;
            _background.size = size;
            _collider.size = size * _colliderSizeMultiplier;
            CurrentHP = startHP;

            OnInit(size, startHP);
        }
        protected abstract void OnInit(Vector2 size, int startHP);

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
        }

        protected virtual void OnCurrentHPChanged(int value)
        {
            OnCellHPChanged?.Invoke(value);
        }
        protected virtual void DestroyCell()
        {
            _collider.enabled = false;

            for (int i = 0; i < _drop.Length; i++)
            {
                var drop = _drop[i];

                if (Random.Range(0f, 1f) <= drop.Chance)
                {
                    var instance = LootDropHelper.Instance.DropOnPosition(drop.Type, transform.position);
                    var dropDirection = Vector2.up + (_isCloseToLeftBound ? Vector2.left : Vector2.right);
                    instance.Rigidbody.AddForce(dropDirection * Random.Range(0.6f, 1.7f), ForceMode2D.Impulse);
                }
            }

            OnCellDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        [System.Serializable]
        public struct DropData
        {
            public LootDropHelper.DropType Type;
            [Range(0f, 1f)] public float Chance;
        }
    }
}

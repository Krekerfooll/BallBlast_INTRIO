using TMPro;
using UnityEngine;

namespace BallBlust.Core
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private TextMeshPro _counter;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Vector2 _counterSizeMultiplier;
        [SerializeField] private Vector2 _colliderSizeMultiplier;

        private int _currentHP;
        public int CurrentHP 
        { 
            get => _currentHP; 
            private set
            {
                _currentHP = value;
                _counter.text = _currentHP.ToString();

                if (_currentHP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Init(Vector2 size, int startHP)
        {
            _background.size = size;
            _counter.rectTransform.sizeDelta = size * _counterSizeMultiplier;
            _collider.size = size * _colliderSizeMultiplier;
            CurrentHP = startHP;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
        }
    }
}

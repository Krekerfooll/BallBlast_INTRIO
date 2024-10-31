using BallBlust.Utils;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BallBlust.Core
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private CellSpawnData[] _cellPrefabs;
        [SerializeField] private float _keepRows;
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private Padding _padding;
        [SerializeField] private Vector2 _spacing;
        [Space]
        [SerializeField] private BoxCollider2D _leftWall;
        [SerializeField] private BoxCollider2D _rightWall;
        [SerializeField] private float _boardSpeed;
        [Space]
        [SerializeField] private Vector2Int _startCellsHPRange;
        [SerializeField] private int _cellsHPStep;

        private Vector2 _cellSize;
        private Vector2 _startPosition;
        private Vector2 _step;

        private int _spawnedRowsCount;

        private void Awake()
        {
            var worldScreenHeight = Camera.main.orthographicSize * 2f;
            var worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

            var fieldHeight = worldScreenHeight - _padding.Top - _padding.Bottom;
            var fieldWidth = worldScreenWidth - _padding.Left - _padding.Right;

            var totalSpacingWidth = _spacing.x * (_boardSize.x - 1);
            var totalSpacingHeight = _spacing.y * (_boardSize.y - 1);

            _cellSize = new Vector2((fieldWidth - totalSpacingWidth) / _boardSize.x, (fieldHeight - totalSpacingHeight) / _boardSize.y);

            var startPositionX = -worldScreenWidth * 0.5f + _padding.Left + _cellSize.x * 0.5f;
            var startPositionY = -worldScreenHeight * 0.5f + _padding.Bottom + _cellSize.y * 0.5f;

            _startPosition = new Vector2(startPositionX, startPositionY);
            _step = new Vector2(_cellSize.x + _spacing.x, _cellSize.y + _spacing.y);

            for (int j = 0; j < _keepRows; j++)
            {
                GenerateRow(_startPosition, _step, j);
            }

            var leftWorldBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
            var rightWorldBound = Camera.main.ViewportToWorldPoint(Vector3.one).x;
            _leftWall.offset = new Vector2(leftWorldBound - _leftWall.size.x * 0.5f, _leftWall.offset.y);
            _rightWall.offset = new Vector2(rightWorldBound + _rightWall.size.x * 0.5f, _rightWall.offset.y);

            StartCoroutine(BoardRoutine());
        }

        private void GenerateRow(Vector2 startPosition, Vector2 step, int height)
        {
            GenerateRow(startPosition.x, step.x, startPosition.y + step.y * height);
        }
        private void GenerateRow(float startPositionX, float stepX, float height)
        {
            for (int i = 0; i < _boardSize.x; i++)
            {
                var cellToSpawn = CellSpawnData.GetRandom(_cellPrefabs);
                var cellPosition = new Vector3(startPositionX + stepX * i, height);
                var instancee = Instantiate(cellToSpawn, cellPosition, Quaternion.identity, transform);
                var currentHPModifier = _cellsHPStep * _spawnedRowsCount;
                var hp = RandomUtils.RandomRange(_startCellsHPRange.x + currentHPModifier, _startCellsHPRange.y + currentHPModifier, 5);
                var isCloseToLeftBound = i <= _boardSize.x * 0.5f;
                instancee.Init(_cellSize, hp, isCloseToLeftBound);
            }

            _spawnedRowsCount++;
        }

        private IEnumerator BoardRoutine()
        {
            var lastPosition = transform.position.y;

            while (gameObject)
            {
                transform.Translate(0, _boardSpeed * Time.deltaTime, 0);

                var passedDistance = lastPosition - transform.position.y;

                if (passedDistance > _cellSize.y + _spacing.y)
                {
                    lastPosition = transform.position.y;
                    var height = _startPosition.y + _step.y * _keepRows - passedDistance;
                    GenerateRow(_startPosition.x, _step.x, height);
                }
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            var worldScreenHeight = Camera.main.orthographicSize * 2f;
            var worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

            var fieldHeight = worldScreenHeight - _padding.Top - _padding.Bottom;
            var fieldWidth = worldScreenWidth - _padding.Left - _padding.Right;
            var centerX = -worldScreenWidth * 0.5f + _padding.Left + fieldWidth * 0.5f;
            var centerY = -worldScreenHeight * 0.5f + _padding.Bottom + fieldHeight * 0.5f;
            var center = new Vector3(centerX, centerY);
            var size = new Vector3(fieldWidth, fieldHeight);

            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(center, size);
        }


        [System.Serializable]
        public struct CellSpawnData
        {
            public BaseCell Cell;
            public int Chance;

            public static BaseCell GetRandom(CellSpawnData[] cells)
            {
                var chanceSum = cells.Sum(cell => cell.Chance);
                var randomValue = Random.Range(0, chanceSum);
                var currentChance = 0;

                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    currentChance += cell.Chance;

                    if (randomValue < currentChance)
                    {
                        return cell.Cell;
                    }
                }

                return cells.Last().Cell;
            }
        }

        [System.Serializable]
        public struct Padding
        {
            public float Left;
            public float Right;
            public float Top;
            public float Bottom;
        }
    }
}

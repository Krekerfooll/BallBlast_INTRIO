using BallBlust.Input;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallBlust.Core
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private Transform _projectilesHolder;
        [SerializeField] private Transform _projectileSpawnPoint;
        [Space]
        [SerializeField] private InputSlider _inputSlider;
        [SerializeField] private Transform _target;
        [SerializeField] private List<Transform> _wheels;
        [Space]
        [SerializeField] private Vector2 _projectilesSpawnBounds;
        [SerializeField] private Vector2 _targetBounds;
        [SerializeField] private Vector2 _wheelRotationBounds;

        private Vector2 _worldBounds;
        private float _worldProjectileDistance;

        private Coroutine _shootingRoutine;
        private Queue<Projectile> _projectilesPool;

        public void Init()
        {
            var camera = Camera.main;
            var leftBound = camera.ViewportToWorldPoint(new Vector3(_targetBounds.x, 0f)).x;
            var rightBound = camera.ViewportToWorldPoint(new Vector3(_targetBounds.y, 0f)).x;
            var topBound = camera.ViewportToWorldPoint(new Vector3(0f, 1f)).y;
            var bottomBound = camera.ViewportToWorldPoint(new Vector3(0f, 0f)).y;

            _worldBounds = new Vector2(leftBound, rightBound);
            _worldProjectileDistance = topBound - bottomBound;

            _projectilesPool = new Queue<Projectile>();

            RotateWheels(0.5f);

            CannonStatsController.OnProjectiletypeChanged += () =>
            {
                StartCoroutine(ResetWeaponRoutine());
            };
        }

        private void OnEnable()
        {
            _inputSlider.OnSliderInput += OnInputChange;
            _shootingRoutine = StartCoroutine(ShootingRoutine());
        }
        private void OnDisable()
        {
            _inputSlider.OnSliderInput += OnInputChange;

            if (_shootingRoutine != null)
            {
                StopCoroutine(_shootingRoutine);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Cell")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnInputChange(float value)
        {
            MoveTarget(value);
            RotateWheels(value);
        }

        private void MoveTarget(float value)
        {
            var targetPosition = Mathf.Lerp(_worldBounds.x, _worldBounds.y, value);

            _target.transform.position = new Vector3(targetPosition, _target.transform.position.y, _target.transform.position.z);
        }
        private void RotateWheels(float value)
        {
            var targetRotaton = Mathf.Lerp(_wheelRotationBounds.x, _wheelRotationBounds.y, value);

            foreach (var wheel in _wheels)
            {
                wheel.rotation = Quaternion.Euler(new Vector3(wheel.rotation.x, wheel.rotation.y, targetRotaton));
            }
        }

        private IEnumerator ShootingRoutine()
        {
            while (gameObject)
            {
                if (_inputSlider.IsPressed)
                {
                    var projectilesStreamsCount = CannonStatsController.CurrentStats.StreamsCount;
                    var currentBounds = new Vector2(
                        _projectileSpawnPoint.position.x + _projectilesSpawnBounds.x,
                        _projectileSpawnPoint.position.x + _projectilesSpawnBounds.y);

                    var spawnRange = currentBounds.y - currentBounds.x;
                    var step = spawnRange / projectilesStreamsCount;

                    for (int i = 0; i < projectilesStreamsCount; i++)
                    {
                        var spawnPos = new Vector3(
                            currentBounds.x + step * i + step * 0.5f,
                            _projectileSpawnPoint.position.y,
                            _projectileSpawnPoint.position.z);

                        if (_projectilesPool.TryDequeue(out var projectile))
                        {
                            projectile.transform.position = spawnPos;
                            projectile.transform.rotation = Quaternion.identity;
                            projectile.gameObject.SetActive(true);
                        }
                        else
                        {
                            projectile = Instantiate(CannonStatsController.CurrentStats.Projectile, spawnPos, Quaternion.identity, _projectilesHolder);
                        }

                        projectile.OnProjectileDestroy += OnProjectileDestroy;
                        projectile.Lounch(CannonStatsController.CurrentStats.ProjectileSpeed, _worldProjectileDistance);
                    }
                }

                yield return new WaitForSeconds(1f / CannonStatsController.CurrentStats.ShootingRate);
            }

            void OnProjectileDestroy(Projectile projectile)
            {
                projectile.OnProjectileDestroy -= OnProjectileDestroy;
                projectile.gameObject.SetActive(false);
                _projectilesPool.Enqueue(projectile);
            }
        }
        private IEnumerator ResetWeaponRoutine()
        {
            if (_shootingRoutine != null)
            {
                StopCoroutine(_shootingRoutine);
            }

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            _projectilesPool = new Queue<Projectile>();

            foreach (Transform projectile in _projectilesHolder)
            {
                Destroy(projectile.gameObject);
            }

            yield return new WaitForEndOfFrame();

            _shootingRoutine = StartCoroutine(ShootingRoutine());
        }
    }
}

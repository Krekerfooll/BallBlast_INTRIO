using BallBlust.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlust.Core
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _shootingRate;
        [Space]
        [SerializeField] private InputSlider _inputSlider;
        [SerializeField] private Transform _target;
        [SerializeField] private List<Transform> _wheels;
        [Space]
        [SerializeField] private Vector2 _targetBounds;
        [SerializeField] private Vector2 _wheelRotationBounds;

        private Vector2 _worldBounds;
        private float _worldProjectileDistance;

        private Coroutine _shootingRoutine;

        private void Awake()
        {
            var camera = Camera.main;
            var leftBound = camera.ViewportToWorldPoint(new Vector3(_targetBounds.x, 0f)).x;
            var rightBound = camera.ViewportToWorldPoint(new Vector3(_targetBounds.y, 0f)).x;
            var topBound = camera.ViewportToWorldPoint(new Vector3(0f, 1f)).y;
            var bottomBound = camera.ViewportToWorldPoint(new Vector3(0f, 0f)).y;

            _worldBounds = new Vector2(leftBound, rightBound);
            _worldProjectileDistance = topBound - bottomBound;

            RotateWheels(0.5f);
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
                    var projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);
                    projectile.Lounch(1, _projectileSpeed, _worldProjectileDistance);
                }

                yield return new WaitForSeconds(1f / _shootingRate);
            }
        }
    }
}

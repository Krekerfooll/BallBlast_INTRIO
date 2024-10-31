using BallBlust.Data;
using System;
using System.Linq;
using UnityEngine;

namespace BallBlust.Core
{
    public class CannonStatsController : MonoBehaviour
    {
        public static Action OnProjectiletypeChanged;

        [SerializeField] private CannonStatsContainer _startStats;
        [SerializeField] private CannonController _cannonController;

        private static CannonStats _currentStats;
        public static CannonStats CurrentStats => _currentStats;

        private void Awake()
        {
            _currentStats = new CannonStats(_startStats);
            _cannonController.Init();
        }

        public static void AddFireRate(float fireRateModifier) => _currentStats.ShootingRate += fireRateModifier;
        public static void AddShieldHP(int shieldHP) => _currentStats.ShieldHP += shieldHP;
        public static void AddStreamsCount(int streamsCountModifier)
        {
            var streamsCount = _currentStats.StreamsCount + streamsCountModifier;

            if (streamsCount > _currentStats.MaxStreamsCount)
            {
                _currentStats.StreamsCount = 1;
                _currentStats.CurrentProjectileIndex++;
                OnProjectiletypeChanged?.Invoke();
            }
            else
            {
                _currentStats.StreamsCount = streamsCount;
            }
        }

        public struct CannonStats
        {
            public float ProjectileSpeed;
            public float ShootingRate;
            public int MaxStreamsCount;
            public int StreamsCount;
            public int ShieldHP;
            public int MaxShieldHP;

            public Projectile Projectile => CurrentProjectileIndex < _projectiles.Length ? _projectiles[CurrentProjectileIndex] : _projectiles.Last();
            public int CurrentProjectileIndex;
            private Projectile[] _projectiles;

            public CannonStats(CannonStatsContainer statsContainer)
            {
                CurrentProjectileIndex = 0;
                _projectiles = statsContainer.Projectiles;
                ProjectileSpeed = statsContainer.ProjectileSpeed;
                ShootingRate = statsContainer.ShootingRate;
                StreamsCount = statsContainer.StreamsCount;
                ShieldHP = statsContainer.ShieldHP;
                MaxShieldHP = statsContainer.MaxShieldHP;
                MaxStreamsCount = statsContainer.MaxStreamsCount;
            }}
    }
}

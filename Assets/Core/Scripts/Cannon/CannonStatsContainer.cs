using BallBlust.Core;
using System.Linq;
using UnityEngine;

namespace BallBlust.Data
{
    [CreateAssetMenu(fileName = "CannonStats", menuName = "GameSettigs/CannonStats", order = 1)]
    public class CannonStatsContainer : ScriptableObject
    {
        [SerializeField] private Projectile[] _projectilePrefabs;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _shootingRate;
        [SerializeField] private int _maxStreamsCount;
        [SerializeField] private int _streamsCount;
        [SerializeField] private int _shieldHP;
        [SerializeField] private int _maxShieldHP;

        public Projectile[] Projectiles => _projectilePrefabs;
        public float ProjectileSpeed => _projectileSpeed;
        public float ShootingRate => _shootingRate;
        public int MaxStreamsCount => _maxStreamsCount;
        public int StreamsCount => _streamsCount;
        public int ShieldHP => _shieldHP;
        public int MaxShieldHP => _maxShieldHP;
    }
}

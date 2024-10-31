using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlust.Core.Drop
{
    public class LootDropHelper : MonoBehaviour
    {
        public enum DropType 
        { 
            Currency_Coin_05 = 10, 
            Currency_Coin_15 = 11,
            Modifier_Shooting_Rate = 30,
            Modifier_Shield = 31,
            Modifier_Projectile_Streams_Count = 32
        }

        public static LootDropHelper Instance { get; private set; }

        [SerializeField] private LootDropData _lootDropData;

        private Dictionary<DropType, Queue<DroppablePoolingObject>> _droppablesPool;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);

                _droppablesPool = new Dictionary<DropType, Queue<DroppablePoolingObject>>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public DroppablePoolingObject DropOnPosition(DropType dropType, Vector3 position)
        {
            var drop = GetFromPool(dropType);
            drop.transform.position = position;

            return drop;
        }

        public DroppablePoolingObject GetFromPool(DropType dropType)
        {
            if (_droppablesPool.TryGetValue(dropType, out var pool) 
                && pool != null 
                && pool.TryDequeue(out var result))
            {
                result.OnGetFromPool();
                return result;
            }
            else
            {
                result = Instantiate(_lootDropData.GetPrefabByType(dropType), transform);
                result.Init(dropType);
                return result;
            }
        }
        public void ReturnToPool(DroppablePoolingObject poolingObject)
        {
            if (_droppablesPool.TryGetValue(poolingObject.DropType, out var pool)) 
            {
                pool.Enqueue(poolingObject);
            }
            else
            {
                var newPool = new Queue<DroppablePoolingObject>();
                newPool.Enqueue(poolingObject);
                _droppablesPool.Add(poolingObject.DropType, newPool);
            }
        }

        [System.Serializable]
        public struct LootDropData
        {
            public DroppablePoolingObject Coin05Currency;
            public DroppablePoolingObject Coin15Currency;
            public DroppablePoolingObject ShootingRateModifier;
            public DroppablePoolingObject ShieldModifier;
            public DroppablePoolingObject ProjectileStreamsCountModifier;

            public DroppablePoolingObject GetPrefabByType(DropType type)
            {
                return type switch
                {
                    DropType.Currency_Coin_05 => Coin05Currency,
                    DropType.Currency_Coin_15 => Coin15Currency,
                    DropType.Modifier_Shooting_Rate => ShootingRateModifier,
                    DropType.Modifier_Shield => ShieldModifier,
                    DropType.Modifier_Projectile_Streams_Count => ProjectileStreamsCountModifier,
                    _ => throw new System.NotImplementedException($"Drop type: \"{type}\" prefab override not implemented")
                };
            }
        }
    }
}

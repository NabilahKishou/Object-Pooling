using System.Collections.Generic;
using UnityEngine;
using Utilites.Singleton;

namespace Utilites.Pooling {
    public class PoolProvider : PersistentSingleton<PoolProvider> {
        [SerializeField] PoolConfig[] _configs;
        Dictionary<string, IObjectPool<GameObject>> _poolDictionary;

        protected override void Awake() {
            base.Awake();
            _poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
            InitPools();
        }

        void InitPools() {
            foreach (var config in _configs) {
                config.CreateContainer(transform);
                var objPool = new ObjectPool<GameObject>.Builder()
                    .WithStartingCapacity(config.defaultCapacity)
                    .WithMaxCapacity(config.maxCapacity)
                    .CreateFactory(config.CreateObject)
                    .WhenGetItem(config.TakenFromPool)
                    .WhenItemReturned(config.ReturnToPool)
                    .WhenItemDisposed(config.DisposedObject)
                    .Build();
                _poolDictionary.Add(config.objectName, objPool);
            }
        }

        public GameObject GetObject(string objectName) => 
            _poolDictionary.TryGetValue(objectName, out var objPool) ? objPool.GetItem() : null;

        public void ReturnObject(string objectName, GameObject obj) {
            if (_poolDictionary.TryGetValue(objectName, out var pool))
                pool.ReleaseItem(obj);
        }
    }
}
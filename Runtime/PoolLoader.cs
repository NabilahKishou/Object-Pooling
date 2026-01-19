using System.Collections.Generic;
using UnityEngine;

namespace Utilites.Pooling {
    public interface IPoolLoader {
        string LoaderName { get; }
        GameObject GetObject(string objectName);
        void ReturnObject(string configName, GameObject obj);
    }
    
    public class PoolLoader : MonoBehaviour, IPoolLoader {
        [SerializeField] string _loaderName;
        [SerializeField] PoolConfig[] _configs;
        Dictionary<string, IObjectPool<GameObject>> _poolDictionary;
        public string LoaderName => _loaderName;

        void Awake() {
            _poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
            InitPools();
            PoolProvider.RegisterLoader(this);
        }

        void OnDestroy() {
            PoolProvider.RemoveLoader(this);
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

        public GameObject GetObject(string objectName) {
            return _poolDictionary.TryGetValue(objectName, out var objPool) ? objPool.GetItem() : null;
        }

        public void ReturnObject(string configName, GameObject obj) {
            if (_poolDictionary.TryGetValue(configName, out var pool))
                pool.ReleaseItem(obj);
        }
    }
}
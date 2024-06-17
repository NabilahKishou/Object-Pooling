using UnityEngine;
using Utilites.Pooling;
using Utilites.Singleton;

namespace Test.Pooling
{
    public class GameObjectPooling : PersistentSingleton<GameObjectPooling>
    {
        [SerializeField] private GameObject _goPrefab;
        [SerializeField] private int _poolSize = 3;
        [SerializeField] private int _maxPoolSize = 5;
        IObjectPool<GameObject> _goPool;

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            _goPool = new ObjectPool<GameObject>.Builder()
                .CreateFactory(InstantiateGO)
                .WithStartingCapacity(_poolSize)
                .WithMaxCapacity(_maxPoolSize)
                .WhenGetItem(TakeFromPool)
                .WhenItemReturned(ReturnedToPool)
                .WhenItemDisposed(DisposedGO)
                .Build();
        }

        private void DisposedGO(GameObject obj)
        {
            Debug.Log("Object disposed because pool is overload!", obj);
            Destroy(obj);
        }

        private void ReturnedToPool(GameObject obj)
        {
            Debug.Log("Object returned from pool!", obj);
            obj.SetActive(false);
        }

        private void TakeFromPool(GameObject obj)
        {
            Debug.Log("Object taken from pool!", obj);
            obj.SetActive(true);
        }

        private GameObject InstantiateGO()
        {
            var go = Instantiate(_goPrefab, this.transform);
            go.SetActive(false);
            return go;
        }

        public GameObject GetObject() => _goPool.GetItem();
        public void ReturnObject(GameObject obj) => _goPool.ReleaseItem(obj);
    }
}
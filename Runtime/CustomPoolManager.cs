using System.Collections.Generic;
using UnityEngine;
using Utilites.Singleton;

namespace Utilites.Pooling
{
    public class CustomPoolManager<T> : PersistentSingleton<CustomPoolManager<T>>
    {
		protected IObjectPool<T> _objectPool;
		protected readonly List<T> _activeObject = new List<T>();

		[SerializeField] protected T _objectPrefab;
		[SerializeField] protected int _defaultPool = 10;
		[SerializeField] protected int _maxPool = 50;
		[SerializeField] protected int _maxObjectInstances = 30;

        private void Start()
        {
			InitializeCustomPool();
        }

		public T GetObject() => _objectPool.GetItem();
		public void ReturnObject(T obj) => _objectPool.ReleaseItem(obj);

		protected virtual void InitializeCustomPool()
        {
			_objectPool = new ObjectPool<T>.Builder()
				.CreateFactory(CreateObject)
				.WhenGetItem(OnTakeFromPool)
				.WhenItemReturned(OnReturnedToPool)
				.WhenItemDisposed(OnDisposedObject)
				.WithStartingCapacity(_defaultPool)
				.WithMaxCapacity(_maxPool)
				.Build();
		}
		protected virtual T CreateObject() { return default; }
		protected virtual void OnTakeFromPool(T obj) { }
		protected virtual void OnReturnedToPool(T obj) { }
		protected virtual void OnDisposedObject(T obj) { }
    }
}
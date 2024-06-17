using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilites.Pooling
{
    public class ObjectPool<T> : IObjectPool<T>
	{
		private readonly List<ItemContainer<T>> _list;
		private readonly Dictionary<T, ItemContainer<T>> _lookup;
		private int _lastIndex = 0;
		private int _defaultPoolSize = 5;
		private int _maxPoolSize = 10;

		private readonly Func<T> _factoryFunc;
		private readonly Action<T> _onTakeFromPool;
		private readonly Action<T> _onReturnedToPool;
		private readonly Action<T> _onObjectDisposed;

		public int Count => _list.Count;
		public int CountUsedItems => _lookup.Count;

		private ObjectPool(Func<T> factoryFunc, Action<T> onGetItem, Action<T> onReleaseItem,
			Action<T> onDisposeItem, int defaultSize, int maxSize)
		{
			this._factoryFunc = factoryFunc;
			this._onTakeFromPool = onGetItem;
			this._onReturnedToPool = onReleaseItem;
			this._onObjectDisposed = onDisposeItem;

			_defaultPoolSize = defaultSize > 1 ? defaultSize : _defaultPoolSize;
			_maxPoolSize = maxSize > _defaultPoolSize ? maxSize : _maxPoolSize;
			_list = new List<ItemContainer<T>>(defaultSize);
			_lookup = new Dictionary<T, ItemContainer<T>>(defaultSize);
			Warm(_defaultPoolSize);
		}

		private void Warm(int capacity)
		{
			for (int i = 0; i < capacity; i++)
			{
				CreateContainer();
			}
		}

		private ItemContainer<T> CreateContainer()
		{
            var container = new ItemContainer<T>{ 
				Item = _factoryFunc() };
            _list.Add(container);
			return container;
		}

		public T GetItem()
		{
			ItemContainer<T> container = null;
			for (int i = 0; i < _list.Count; i++)
			{
				_lastIndex++;
				if (_lastIndex > _list.Count - 1) _lastIndex = 0;

				if (_list[_lastIndex].Used)
				{
					continue;
				}
				else
				{
					container = _list[_lastIndex];
					break;
				}
			}

			if (container == null)
			{
				container = CreateContainer();
			}

			container.Caught();
			_onTakeFromPool(container.Item);
			if(_lookup.Count < _maxPoolSize)
				_lookup.Add(container.Item, container);
			return container.Item;
		}

		public void ReleaseItem(object item)
		{
			ReleaseItem((T)item);
		}

		public void ReleaseItem(T item)
		{
			if (_lookup.ContainsKey(item))
			{
				_lookup[item].Release();
				_onReturnedToPool(item);
				_lookup.Remove(item);
			}
			else
			{
				Debug.LogWarning("This object pool does not contain the item provided so we dispose of it!");
				_onObjectDisposed(item);
			}
		}

		public class Builder
        {
			private int _defSize = 0;
			private int _maxSize = 0;
			private Func<T> _factoryFunc;
			private Action<T> _onTakeFromPool = delegate { };
			private Action<T> _onReturnedToPool = delegate { };
			private Action<T> _onObjectDisposed = delegate { };
			
			public ObjectPool<T> Build() =>
				new ObjectPool<T>(_factoryFunc, _onTakeFromPool, _onReturnedToPool, _onObjectDisposed, _defSize, _maxSize);
			public Builder CreateFactory(Func<T> func) { _factoryFunc = func; return this; }
			public Builder WithStartingCapacity(int startCapacity) { _defSize = startCapacity; return this; }
			public Builder WithMaxCapacity(int maxCapacity) { _maxSize = maxCapacity; return this; }
			public Builder WhenGetItem(Action<T> act) { _onTakeFromPool = act; return this; }
			public Builder WhenItemReturned(Action<T> act) { _onReturnedToPool = act; return this; }
			public Builder WhenItemDisposed(Action<T> act) { _onObjectDisposed = act; return this; }
		}
	}

    public interface IObjectPool<T>
    {
		T GetItem();
		void ReleaseItem(T item);
    }
}
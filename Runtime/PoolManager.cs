﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utilites.Singleton;

namespace Utilites.Pooling
{
    public class PoolManager : Singleton<PoolManager>
	{
		public bool _logStatus;
		public Transform _root;

		private Dictionary<GameObject, ObjectPool<GameObject>> _prefabLookup;
		private Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup;

		private bool _dirty = false;

		protected override void Awake()
		{
			_prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
			_instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
		}

		void Update()
		{
			if (_logStatus && _dirty)
			{
				PrintStatus();
				_dirty = false;
			}
		}

		public void warmPool(GameObject prefab, int size)
		{
			if (_prefabLookup.ContainsKey(prefab))
			{
				throw new Exception("Pool for prefab " + prefab.name + " has already been created");
			}
			var pool = new ObjectPool<GameObject>.Builder()
				.CreateFactory(()=>InstantiatePrefab(prefab)).WithStartingCapacity(size).Build();
			_prefabLookup[prefab] = pool;

			_dirty = true;
		}

		public GameObject spawnObject(GameObject prefab)
		{
			return spawnObject(prefab, Vector3.zero, Quaternion.identity);
		}

		public GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (!_prefabLookup.ContainsKey(prefab))
			{
				WarmPool(prefab, 1);
			}

			var pool = _prefabLookup[prefab];

			var clone = pool.GetItem();
			clone.transform.SetPositionAndRotation(position, rotation);
			clone.SetActive(true);

			_instanceLookup.Add(clone, pool);
			_dirty = true;
			return clone;
		}

		public void releaseObject(GameObject clone)
		{
			clone.SetActive(false);

			if (_instanceLookup.ContainsKey(clone))
			{
				_instanceLookup[clone].ReleaseItem(clone);
				_instanceLookup.Remove(clone);
				_dirty = true;
			}
			else
			{
				Debug.LogWarning("No pool contains the object: " + clone.name);
			}
		}


		private GameObject InstantiatePrefab(GameObject prefab)
		{
			var go = Instantiate(prefab) as GameObject;
			if (_root != null) go.transform.parent = _root;
			return go;
		}

		public void PrintStatus()
		{
			foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in _prefabLookup)
			{
				Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name, keyVal.Value.CountUsedItems, keyVal.Value.Count));
			}
		}

		#region Static API

		public static void WarmPool(GameObject prefab, int size)
		{
			Instance.warmPool(prefab, size);
		}

		public static GameObject SpawnObject(GameObject prefab)
		{
			return Instance.spawnObject(prefab);
		}

		public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return Instance.spawnObject(prefab, position, rotation);
		}

		public static void ReleaseObject(GameObject clone)
		{
			Instance.releaseObject(clone);
		}

		#endregion
	}
}
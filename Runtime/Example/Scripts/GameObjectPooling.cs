using UnityEngine;
using Utilites.Pooling;

namespace ObjectPool.Example
{
    public class GameObjectPooling : CustomPoolManager<GameObject>
    {
        protected override GameObject CreateObject()
        {
            var go = Instantiate(_objectPrefab, this.transform);
            go.SetActive(false);
            return go;
        }

        protected override void OnTakeFromPool(GameObject obj)
        {
            Debug.Log("Object taken from pool!", obj);
            obj.SetActive(true);
        }

        protected override void OnReturnedToPool(GameObject obj)
        {
            Debug.Log("Object returned to pool!", obj);
            obj.SetActive(false);
        }

        protected override void OnDisposedObject(GameObject obj)
        {
            Debug.Log("Object disposed because pool is overload!", obj);
            Destroy(obj);
        }
    }
}
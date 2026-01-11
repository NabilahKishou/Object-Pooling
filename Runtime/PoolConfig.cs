using UnityEngine;

namespace Utilites.Pooling {
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Pool/Object Pool Config")]
    public class PoolConfig : ScriptableObject {
        public string objectName = "DefaultObject";
        public int maxCapacity = 10;
        public int defaultCapacity = 5;
        public GameObject objectPrefab;
        Transform _poolContainer;

        public void CreateContainer(Transform parent) {
           _poolContainer = new GameObject($"{objectPrefab.name} Pool") {
                transform = {
                    parent = parent,
                },
            }.transform;
        }

        public virtual GameObject CreateObject() {
            if (_poolContainer == null) CreateContainer(default);
            var go = Instantiate(objectPrefab, _poolContainer);
            go.SetActive(false);
            return go;
        }

        public virtual void TakenFromPool(GameObject go) {
            go.SetActive(true);
        }

        public virtual void ReturnToPool(GameObject go) {
            go.SetActive(false);
        }

        public virtual void DisposedObject(GameObject go) {
            Destroy(go);
        }
    }
}
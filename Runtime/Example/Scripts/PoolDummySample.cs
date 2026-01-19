using System.Collections.Generic;
using UnityEngine;
using Utilites.Pooling;

namespace Utilities.Pooling.Example {
    public class DummyObject {
        public IPoolLoader loader;
        public string configName;
        public GameObject gameObject;
    }
    
    public class PoolDummySample : MonoBehaviour {
        [SerializeField] string[] _objects;
        Stack<DummyObject> _stackObjects = new Stack<DummyObject>();
        string _latestObjName;
        GameObject _latestobject;
    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SpaceKeyDown();
            if (Input.GetKeyDown(KeyCode.Backspace))
                BackspaceDown();
        }

        void BackspaceDown() {
            var dummy = _stackObjects.Pop();
            dummy.loader.ReturnObject(dummy.configName, dummy.gameObject);
        }

        void SpaceKeyDown() {
            SpawnAndReposition();
        }

        void SpawnAndReposition() {
            var random = _objects[Random.Range(0, _objects.Length)];
            var loader = PoolProvider.GetLoader("TestLoader");
            var go = loader.GetObject(random);
            var maxRange = 3f;
            go.transform.position = new Vector3(Random.Range(-maxRange, maxRange), 
                Random.Range(-maxRange, maxRange), Random.Range(-maxRange, maxRange));
            var dummy = new DummyObject() {
                loader = loader,
                configName = random,
                gameObject = go,
            };
            _stackObjects.Push(dummy);
        }
    }
}

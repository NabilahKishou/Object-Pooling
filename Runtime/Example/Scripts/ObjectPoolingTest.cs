using UnityEngine;
using Utilites.Pooling;

namespace ObjectPool.Example
{
    public class ObjectPoolingTest : MonoBehaviour
    {
        [SerializeField] private GameObject _latestobject;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SpaceKeyDown();
            if (Input.GetKeyDown(KeyCode.Backspace))
                BackspaceDown();
        }

        private void BackspaceDown()
        {
            PoolProvider.Instance.ReturnObject("DefaultObject", _latestobject);
        }

        private void SpaceKeyDown()
        {
            _latestobject = PoolProvider.Instance.GetObject("DefaultObject");
            _latestobject.GetComponent<PoolingObject>().ObjectOn();
        }
    }
}
using UnityEngine;

namespace Test.Pooling
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
            GameObjectPooling.Instance.ReturnObject(_latestobject);
        }

        private void SpaceKeyDown()
        {
            _latestobject = GameObjectPooling.Instance.GetObject();
            _latestobject.GetComponent<PoolingObject>().ObjectOn();
        }
    }
}
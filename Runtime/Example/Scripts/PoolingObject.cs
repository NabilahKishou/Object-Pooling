using System.Collections;
using UnityEngine;

namespace Test.Pooling
{
    public class PoolingObject : MonoBehaviour
    {
        [SerializeField] private float _lifeDuration = 3f;
        private Coroutine _lifeCoroutine;

        private IEnumerator WaitForLifeCycle()
        {
            yield return new WaitForSeconds(_lifeDuration);
            ObjectOff();
        }

        public void ObjectOn()
        {
            if (_lifeCoroutine != null) StopCoroutine(_lifeCoroutine);
            _lifeCoroutine = StartCoroutine(WaitForLifeCycle());
        }
        
        public void ObjectOff()
        {
            if(_lifeCoroutine != null)
            {
                StopCoroutine(_lifeCoroutine);
                _lifeCoroutine = null;
            }

            GameObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }
}
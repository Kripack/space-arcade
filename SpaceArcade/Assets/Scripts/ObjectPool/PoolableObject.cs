using UnityEngine;

namespace SpaceArcade.ObjectPool
{
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        [SerializeField] float lifetime = 5f;
        [SerializeField] bool autoReturnToPool = true;
        public virtual void OnSpawn()
        {
            CancelInvoke(nameof(ReturnToPool));
            
            if (autoReturnToPool)
            {
                Invoke(nameof(ReturnToPool), lifetime);
            }
        }

        public virtual void OnReturn()
        {
            CancelInvoke(nameof(ReturnToPool));
        }

        void OnEnable()
        {
            OnSpawn();
        }

        void OnDisable()
        {
            OnReturn();
        }
        
        protected void ReturnToPool()
        {
            PoolManager.Instance.Return(gameObject);
        }

        public void SetLifetime(float time)
        {
            lifetime = time;
            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), lifetime); 
        }
    }
}

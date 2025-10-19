using SpaceArcade.ObjectPool;
using UnityEngine;

namespace SpaceArcade.Managers
{
    public class VisualFXManager : MonoBehaviour
    {
        public static VisualFXManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } 
            else 
            {
                Instance = this;
            }
        }
        
        public void SpawnEffect(GameObject impactObject, Vector3 spawnPosition, Quaternion rotation, float lifetime = 5f)
        {
            if (!impactObject) return;
            
            GameObject effect = PoolManager.Instance.SpawnWithAutoReturn(
                impactObject,
                spawnPosition,
                rotation,
                lifetime
            );
        }
    }
}

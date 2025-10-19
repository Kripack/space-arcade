using System.Collections.Generic;
using UnityEngine;

namespace SpaceArcade.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [Header("Pool Settings")]
        [SerializeField] int defaultPoolSize = 20;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] Transform poolParent;

        Dictionary<int, Queue<GameObject>> _pools = new Dictionary<int, Queue<GameObject>>();
        Dictionary<int, GameObject> _prefabRegistry = new Dictionary<int, GameObject>();
        Dictionary<GameObject, int> _activeObjects = new Dictionary<GameObject, int>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if (poolParent == null)
            {
                GameObject parent = new GameObject("PoolParent");
                poolParent = parent.transform;
                poolParent.SetParent(transform);
            }
        }

        int GetPrefabId(GameObject prefab)
        {
            return prefab.name.GetHashCode();
        }

        public void PrewarmPool(GameObject prefab, int count)
        {
            if (prefab == null) return;

            int prefabId = GetPrefabId(prefab);

            if (!_pools.ContainsKey(prefabId))
            {
                _pools[prefabId] = new Queue<GameObject>();
                _prefabRegistry[prefabId] = prefab;
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = CreateNewObject(prefab, prefabId);
                obj.SetActive(false);
                _pools[prefabId].Enqueue(obj);
            }
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                Debug.LogError("PoolManager: Trying to spawn null prefab");
                return null;
            }

            int prefabId = GetPrefabId(prefab);

            if (!_pools.ContainsKey(prefabId))
            {
                _pools[prefabId] = new Queue<GameObject>();
                _prefabRegistry[prefabId] = prefab;
                PrewarmPool(prefab, defaultPoolSize);
            }

            GameObject obj = GetObjectFromPool(prefabId);

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            _activeObjects[obj] = prefabId;

            return obj;
        }
        
        public GameObject SpawnWithAutoReturn(GameObject prefab, Vector3 position, Quaternion rotation, float lifetime = 5f)
        {
            GameObject obj = Spawn(prefab, position, rotation);
        
            var autoReturn = obj.GetComponent<PoolableObject>();
            autoReturn.SetLifetime(lifetime);

            return obj;
        }
        
        public T Spawn<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            GameObject obj = Spawn(prefab, position, rotation);
            return obj != null ? obj.GetComponent<T>() : null;
        }
        
        public void Return(GameObject obj)
        {
            if (obj == null) return;

            if (!_activeObjects.TryGetValue(obj, out int prefabId))
            {
                Debug.LogWarning($"PoolManager: Object {obj.name} not tracked by pool, destroying instead");
                Destroy(obj);
                return;
            }

            _activeObjects.Remove(obj);

            if (_pools[prefabId].Count < maxPoolSize)
            {
                obj.SetActive(false);
                obj.transform.SetParent(poolParent);
                _pools[prefabId].Enqueue(obj);
            }
            else
            {
                Destroy(obj);
            }
        }

        GameObject GetObjectFromPool(int prefabId)
        {
            while (_pools[prefabId].Count > 0)
            {
                GameObject obj = _pools[prefabId].Dequeue();
                if (obj != null)
                {
                    return obj;
                }
            }

            return CreateNewObject(_prefabRegistry[prefabId], prefabId);
        }

        GameObject CreateNewObject(GameObject prefab, int prefabId)
        {
            GameObject obj = Instantiate(prefab, poolParent);
            obj.name = $"{prefab.name} (Pooled)";

            var poolable = obj.GetComponent<IPoolable>();
            if (poolable == null)
            {
                obj.AddComponent<PoolableObject>();
            }
            
            obj.SetActive(false);
            
            return obj;
        }

        public void ClearPool(GameObject prefab)
        {
            if (prefab == null) return;

            int prefabId = GetPrefabId(prefab);

            if (_pools.ContainsKey(prefabId))
            {
                while (_pools[prefabId].Count > 0)
                {
                    GameObject obj = _pools[prefabId].Dequeue();
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                _pools.Remove(prefabId);
                _prefabRegistry.Remove(prefabId);
            }
        }

        public void ClearAllPools()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    GameObject obj = pool.Dequeue();
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
            }
            _pools.Clear();
            _prefabRegistry.Clear();
            _activeObjects.Clear();
        }

        void OnDestroy()
        {
            ClearAllPools();
        }
    }
}

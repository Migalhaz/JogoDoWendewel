using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Pool
{
    [Serializable]
    public class Pool 
    {
        #region Variables
        [SerializeField] PoolDataScriptableObject m_poolData;

        Transform m_poolParent;
        List<GameObject> m_freeObjects;
        List<GameObject> m_inUseObjects;
        #region Getters
        public PoolDataScriptableObject m_PoolData => m_poolData;
        public List<GameObject> m_FreeObjects => m_freeObjects;
        public List<GameObject> m_InUseObjects => m_inUseObjects;
        #endregion

        #endregion

        #region Methods
        public bool CompareTag(string _tag)
        {
            return m_poolData.CompareTag(_tag);
        }

        public void Setup(Transform _parent)
        {
            m_freeObjects = new List<GameObject>();
            m_inUseObjects = new List<GameObject>();
            m_poolParent = _parent;
        }

        public void CreateObject()
        {
            GameObject newGameObject = UnityEngine.Object.Instantiate(m_poolData.m_Prefab, m_poolParent);
            newGameObject.SetActive(false);
            m_freeObjects.Add(newGameObject);
        }

        public GameObject GetFreeGameObject()
        {
            if (AllowCreateNewObject())
            {
                CreateObject();
            }

            GameObject poolGameObject = m_FreeObjects[m_FreeObjects.Count - 1];
            m_freeObjects.Remove(poolGameObject);
            m_inUseObjects.Add(poolGameObject);
            poolGameObject.SetActive(true);
            return poolGameObject;
        }

        bool AllowCreateNewObject()
        {
            if (m_poolParent.childCount <= 0) return true;
            if (m_freeObjects.Count > 0) return false;
            if (m_poolParent.childCount >= m_poolData.m_PoolSize)
            {
                if (!m_poolData.m_ExpandablePool)
                {
                    return false;
                }
            }
            return true;
        }

        public List<GameObject> GetAllGameObjects()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.AddRange(m_InUseObjects);
            gameObjects.AddRange(m_FreeObjects);
            return gameObjects;
        }

        public List<T> GetAllGameObjects<T>() where T : Component
        {
            List<T> t = new();
            GetAllGameObjects().ForEach(x => t.Add(x.GetComponent<T>()));
            return t;
        }

        public void ReturnObject(GameObject _activeGameObject)
        {
            if (!m_inUseObjects.Contains(_activeGameObject)) return;

            m_freeObjects.Add(_activeGameObject);
            m_inUseObjects.Remove(_activeGameObject);
            _activeGameObject.SetActive(false);

            if (!m_poolData.m_DestroyExtraObjectsAfterUse) return;
            
            if (m_poolParent.childCount <= m_poolData.m_PoolSize) return;
            UnityEngine.Object.Destroy(_activeGameObject);
            m_freeObjects.Remove(_activeGameObject);
        }


        public void ReturnAlllObjects()
        {
            m_freeObjects.ForEach(x => ReturnObject(x));
            //m_freeObjects.AddRange(m_inUseObjects);
            //m_inUseObjects.Clear();
            //m_freeObjects.ForEach(x => x.SetActive(false));
        }

        
        #endregion
    }
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] List<Pool> m_pools = new List<Pool>() { new Pool()};

        protected override void Awake()
        {
            base.Awake();

            foreach (Pool pool in m_pools)
            {
                pool.Setup(new GameObject($"{pool.m_PoolData.m_PoolTag} Pool").transform);
                for (int i = 0; i < pool.m_PoolData.m_PoolSize; i++)
                {
                    pool.CreateObject();
                }
            }
        }
        public Pool GetPool(string _poolTag)
        {
            return m_pools.Find(x => x.CompareTag(_poolTag));
        }

        public Pool GetPool(PoolDataScriptableObject _poolData)
        {
            //return m_pools.Find(x => x.m_PoolData == _poolData);
            return GetPool(_poolData.m_PoolTag);
        }

        public GameObject GetFreeGameObjectFromPool(string _poolTag)
        {
            return GetPool(_poolTag).GetFreeGameObject();
        }

        public GameObject GetFreeGameObjectFromPool(PoolDataScriptableObject _poolTag)
        {
            return GetPool(_poolTag).GetFreeGameObject();
        }

        public T GetFreeGameObjectFromPool<T>(string _poolTag) where T : Component
        {
            GameObject g = GetFreeGameObjectFromPool(_poolTag);
            if (g == null) return null;

            if (g.TryGetComponent(out T _t))
            {
                return _t;
            }

            return null;
        }

        public void ReturnUsingGameObjectToPool(string _poolTag, GameObject _gameObject)
        {
            GetPool(_poolTag).ReturnObject(_gameObject);
        }

        public void ReturnUsingGameObjectToPool(PoolDataScriptableObject _poolTag, GameObject _gameObject)
        {
            GetPool(_poolTag).ReturnObject(_gameObject);
        }

        public void ReturnAllObjectsToPool(string _poolTag)
        {
            GetPool(_poolTag).ReturnAlllObjects();
        }
    }
}

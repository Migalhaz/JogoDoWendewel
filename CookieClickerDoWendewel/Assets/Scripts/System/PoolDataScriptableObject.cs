using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.Pool 
{
    [CreateAssetMenu(fileName = "New Pool Data", menuName = "Scriptable Object/Pool/New Pool")]
    public class PoolDataScriptableObject : ScriptableObject
    {
        [SerializeField] string m_poolTag = "New Pool";
        [SerializeField] GameObject m_prefab;
        [SerializeField, Min(1)] int m_poolSize = 1;
        [SerializeField] bool m_expandablePool = false;
        [SerializeField] bool m_destroyExtraObjectsAfterUse;

        public string m_PoolTag => m_poolTag;
        public int m_PoolSize => m_poolSize;
        public bool m_ExpandablePool => m_expandablePool;
        public bool m_DestroyExtraObjectsAfterUse => m_destroyExtraObjectsAfterUse;
        public GameObject m_Prefab => m_prefab;

        public bool CompareTag(string _tag)
        {
            return m_poolTag.Equals(_tag);
        }
    }
}
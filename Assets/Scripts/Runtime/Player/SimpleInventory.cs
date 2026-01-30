using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.ScriptableObjects;

namespace InteractionSystem.Runtime.Player
{
    public class SimpleInventory : MonoBehaviour
    {
        private List<KeyData> m_Keys = new List<KeyData>();

        public void AddKey(KeyData key)
        {
            if (!m_Keys.Contains(key))
            {
                m_Keys.Add(key);
                Debug.Log($"Inventory: Added {key.KeyName}");
            }
        }

        public bool HasKey(KeyData key)
        {
            return m_Keys.Contains(key);
        }
    }
}
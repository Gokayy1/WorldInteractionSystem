using System; // Action için gerekli
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.ScriptableObjects;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Oyuncunun altın ve anahtar envanterini yönetir.
    /// Değişikliklerde UI'ı bilgilendirir.
    /// </summary>
    public class SimpleInventory : MonoBehaviour
    {
        // Envanter değiştiğinde tetiklenecek event
        public event Action OnInventoryChanged;

        private Dictionary<KeyData, int> m_Keys = new Dictionary<KeyData, int>();
        
        private int m_Gold = 0;

        public int Gold => m_Gold;
        public Dictionary<KeyData, int> Keys => m_Keys;

        public void AddGold(int amount)
        {
            if (amount > 0)
            {
                m_Gold += amount;
                Debug.Log($"Inventory: Added {amount} Gold. Total: {m_Gold}");
                OnInventoryChanged?.Invoke();
            }
        }

        /// <summary>
        /// Envantere anahtar ekler.
        /// </summary>
        public void AddKey(KeyData key)
        {
            if (key != null)
            {
                if (m_Keys.ContainsKey(key))
                {
                    m_Keys[key]++;
                }
                else
                {
                    m_Keys.Add(key, 1);
                }
                
                Debug.Log($"Inventory: Added {key.KeyName}. Count: {m_Keys[key]}");
                OnInventoryChanged?.Invoke();
            }
        }

        /// <summary>
        /// Belirli bir anahtara sahip mi?
        /// </summary>
        public bool HasKey(KeyData key)
        {
            return key != null && m_Keys.ContainsKey(key) && m_Keys[key] > 0;
        }

        public void RemoveKey(KeyData key)
        {
            if (key != null && m_Keys.ContainsKey(key))
            {
                m_Keys[key]--; 

                if (m_Keys[key] <= 0)
                {
                    m_Keys.Remove(key);
                }

                Debug.Log($"Inventory: Used {key.KeyName}. Remaining: {(m_Keys.ContainsKey(key) ? m_Keys[key] : 0)}");
                
                // UI'ı güncelle
                OnInventoryChanged?.Invoke();
            }
        }
    }
}
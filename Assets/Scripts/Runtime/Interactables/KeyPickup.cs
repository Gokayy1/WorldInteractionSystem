using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.ScriptableObjects;

namespace InteractionSystem.Runtime.Interactables
{
    public class KeyPickup : InteractableBase
    {
        [Header("Key Settings")]
        [SerializeField] private KeyData m_KeyData;

        public override bool OnInteract(GameObject interactor)
        {
            if (m_KeyData == null) return false;

            var inventory = interactor.GetComponent<SimpleInventory>();
            if (inventory != null)
            {
                inventory.AddKey(m_KeyData);
                
                // Efekt veya ses eklenebilir
                
                // Nesneyi yok et
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        protected override string GetCurrentPrompt()
        {
            return m_KeyData != null ? $"Press 'E' to Collect {m_KeyData.KeyName}" : "Unknown Key";
        }
    }
}
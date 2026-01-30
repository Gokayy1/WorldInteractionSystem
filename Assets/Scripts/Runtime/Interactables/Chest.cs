using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Basılı tutularak (Hold) açılan sandık.
    /// İçinden rastgele veya belirli miktarda altın çıkar.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Chest : InteractableBase
    {
        #region Serialized Fields

        [Header("Chest Settings")]
        [Tooltip("Sandıktan çıkacak altın miktarı.")]
        [SerializeField] private int m_GoldAmount = 100;

        #endregion

        #region Private Fields

        private Animator m_Animator;
        private bool m_IsOpen = false;
        private const string k_AnimParamOpen = "IsOpen";

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            // Varsayılan Hold süresini ayarla (Base class'taki değer)
            if (m_HoldDuration <= 0f) m_HoldDuration = 2.0f; // Default 2 saniye
        }

        #endregion

        #region Interactable Overrides

        public override bool OnInteract(GameObject interactor)
        {
            // Zaten açıksa ve tekrar etkileşime kapalıysa işlem yapma
            if (m_IsOpen) return false;

            // Kilit kontrolü (Base)
            if (!base.OnInteract(interactor)) return false;

            // Sandığı aç
            OpenChest(interactor);
            
            return true;
        }

        protected override string GetCurrentPrompt()
        {
            if (m_IsOpen) return "Opened";
            return $"Hold 'E' to Open ({m_HoldDuration}s)";
        }

        #endregion

        #region Custom Methods

        private void OpenChest(GameObject interactor)
        {
            if (m_IsOpen) return;

            m_IsOpen = true;

            // Animasyonu oynat
            if (m_Animator != null)
            {
                m_Animator.SetBool(k_AnimParamOpen, true);
            }

            // Oyuncuya altın ver
            var inventory = interactor.GetComponent<SimpleInventory>();
            if (inventory != null)
            {
                inventory.AddGold(m_GoldAmount);
                Debug.Log($"Chest Opened! Found {m_GoldAmount} Gold.");
            }

            // Ses efekti çalınabilir (TODO)
        }

        #endregion
    }
}
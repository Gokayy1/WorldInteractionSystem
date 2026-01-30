using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player; // Inventory erişimi için
using InteractionSystem.Runtime.ScriptableObjects; // KeyData için

namespace InteractionSystem.Runtime.Interactables
{
    [RequireComponent(typeof(Animator))]
    public class Door : InteractableBase
    {
        #region Serialized Fields

        [Header("Door Settings")]
        [Tooltip("Bu kapıyı açmak için gereken anahtar (Boş ise anahtar istemez).")]
        [SerializeField] private KeyData m_RequiredKey;

        #endregion

        #region Private Fields

        private Animator m_Animator;
        private bool m_IsOpen = false;
        private const string k_AnimParamOpen = "IsOpen";

        #endregion

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            if (m_IsLocked && m_RequiredKey == null)
            {
                Debug.LogWarning($"[{name}] Kapı kilitli ama anahtar atanmamış! Asla açılamaz.", this);
            }
        }

        public override bool OnInteract(GameObject interactor)
        {
            // Kilitli mi kontrol et
            if (m_IsLocked)
            {
                // Anahtar kontrolü yap
                if (m_RequiredKey != null)
                {
                    var inventory = interactor.GetComponent<SimpleInventory>();
                    if (inventory != null && inventory.HasKey(m_RequiredKey))
                    {
                        inventory.RemoveKey(m_RequiredKey);

                        Debug.Log($"Unlocked using {m_RequiredKey.KeyName}");
                        m_IsLocked = false;
                    }
                    else
                    {
                        // Anahtar yok
                        Debug.Log("Requires Key!");
                        // TODO: Ekrana "Need Red Key" yazısı basılabilir (Event ile)
                        return false; 
                    }
                }
                else
                {
                    // Kilitli ama anahtar tanımlı değil (belki lever ile açılacak)
                    return false;
                }
            }

            // Normal Toggle işlemi (Artık kilitli değilse burası çalışır)
            m_IsOpen = !m_IsOpen;
            if (m_Animator != null) m_Animator.SetBool(k_AnimParamOpen, m_IsOpen);

            return true;
        }

        protected override string GetCurrentPrompt()
        {
            if (m_IsLocked)
            {
                return m_RequiredKey != null ? $"Locked ({m_RequiredKey.KeyName} Required)" : "Locked";
            }
            return m_IsOpen ? "Press 'E' to Close" : "Press 'E' to Open";
        }

        #region Public Methods

        /// <summary>
        /// Kapının durumunu dışarıdan (örn: Switch ile) değiştirmek için kullanılır.
        /// </summary>
        public void SetState(bool isOpen)
        {
            if (m_IsLocked && isOpen) return; // Kilitliyse açmaya zorlama

            m_IsOpen = isOpen;
            if (m_Animator != null)
            {
                m_Animator.SetBool(k_AnimParamOpen, m_IsOpen);
            }
        }

        public void SetLocked(bool isLocked)
        {
            m_IsLocked = isLocked;

            if (m_IsLocked && m_IsOpen)
            {
                SetState(false);
            }
        }

        public void SetRequiredKey(KeyData key)
        {
            m_RequiredKey = key;
        }

        #endregion
    }
}